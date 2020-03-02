USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_Get_Event_Groups]    Script Date: 3/20/2019 3:59:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Brian Cavanaugh (Callibrity)
-- Create date: 03/12/2019
-- Description:	Get Event Groups with 
-- participant counts
-- =============================================
ALTER    PROCEDURE [dbo].[api_crds_Get_Event_Groups]
	@EventId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EventDate DATETIME;
	
	-- Below line is for testing only
	-- SET @EventId = 4588680;
	
	DECLARE @tblRawGroups AS TABLE
	(
		Group_ID INT,
		Group_Name NVARCHAR(100),
		[Description] NVARCHAR(100),
		Event_ID INT,
		Group_Participants INT,
		Checked_In INT
	)
	
	SELECT @EventDate = Event_Start_Date
	FROM [Events]
	WHERE Event_ID = @EventId
	
	-- In addition to selecting 'normal' group records, add a summary record of all groups combined
	INSERT INTO @tblRawGroups
	SELECT 0
		, 'All Participants in All Groups'
		, 'All Participants in All Groups'
		, @EventId as Event_ID
		, COUNT(*) AS Group_Participants
		, SUM(CASE ep.Participation_Status_ID WHEN 3 THEN 1 ELSE 0 END) AS Checked_In
	FROM Groups g
		JOIN Group_Participants gp ON gp.Group_ID = g.Group_ID
		LEFT JOIN Event_Participants ep ON ep.Event_ID = @EventId AND ep.Group_Participant_ID = gp.Group_Participant_ID AND ep.Group_ID = gp.Group_ID
	WHERE g.Group_ID IN (SELECT Group_ID
							FROM Event_Groups
							WHERE Event_ID = @EventId
						)
		AND g.Event_CheckIn = 1
		AND (gp.End_Date is null OR gp.End_Date > @EventDate)

	-- Select 'normal' group records
	INSERT INTO @tblRawGroups
	SELECT g.Group_ID
		, g.Group_Name
		, g.[Description]
		, @EventId as Event_ID
		, COUNT(*) AS Group_Participants
		, SUM(CASE ep.Participation_Status_ID WHEN 3 THEN 1 ELSE 0 END) AS Checked_In
	FROM Groups g
		JOIN Group_Participants gp ON gp.Group_ID = g.Group_ID
		LEFT JOIN Event_Participants ep ON ep.Event_ID = @EventId
			AND ep.Group_Participant_ID = gp.Group_Participant_ID 
			AND ep.Group_ID = gp.Group_ID
	WHERE g.Group_ID IN (SELECT Group_ID
							FROM Event_Groups
							WHERE Event_ID = @EventId
						)
		AND g.Event_CheckIn = 1
		AND (gp.End_Date is null OR gp.End_Date > @EventDate)
	GROUP BY g.Group_ID, g.Group_Name, g.[Description]

	SELECT
		Group_ID,
		Group_Name,
		[Description],
		Event_ID,
		Group_Participants,
		Checked_In	
	FROM @tblRawGroups

END

GO


DECLARE @IDs TABLE (ID INT)
-- add the proc and permissions to the mp db for 'api_crds_Get_Event_Groups'
IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_Get_Event_Groups')
BEGIN	

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	OUTPUT INSERTED.API_Procedure_ID INTO @IDs
	VALUES('api_crds_Get_Event_Groups')

	IF NOT EXISTS(select * from [dbo].[dp_Role_API_Procedures] WHERE API_Procedure_ID = (SELECT TOP(1) * FROM @IDs) AND Role_ID = 62)
	BEGIN
		INSERT INTO [dbo].[dp_Role_API_Procedures]
		([Role_ID],
		[API_Procedure_ID],
		[Domain_ID])
		VALUES
		(62,
		(SELECT TOP(1) * FROM @IDs),
		1)
	END
END


GO
