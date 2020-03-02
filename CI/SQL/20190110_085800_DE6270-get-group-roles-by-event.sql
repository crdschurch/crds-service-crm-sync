USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_Get_Leader_Events]    Script Date: 11/16/2018 9:26:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Brian Cavanaugh (Callibrity)
-- Create date: 11/26/2018
-- Description:	Get Group Participant record with 
-- Group Role ID by Contact ID and Event ID
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[api_crds_Get_Group_Roles_By_Event]
	@ContactId INT,
	@EventId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT a.Group_ID,
		a.Participant_ID,
		a.Contact_ID,
		a.Nickname,
		a.Last_Name,
		MAX(CASE WHEN a.Group_Role_ID = 22 THEN 1 ELSE 0 END) as 'Is_Leader',
		MAX(CASE WHEN a.Group_Role_ID = 16 THEN 1 ELSE 0 END) as 'Is_Member'
	FROM (SELECT gp.Group_Participant_ID,
				gp.Group_ID,
				gp.Participant_ID,
				c.Contact_ID,
				c.Nickname,
				c.Last_Name,
				gp.Group_Role_ID
			FROM Group_Participants gp
				JOIN Participants p ON p.Participant_ID = gp.Participant_ID
				JOIN Contacts c ON c.Contact_ID = p.Contact_ID
				JOIN Groups g ON g.Group_ID = gp.Group_ID
				JOIN Event_Groups eg ON eg.Group_ID = g.Group_ID
			WHERE c.Contact_ID = @ContactId
				AND eg.Event_ID = @EventId
		) a
	GROUP BY a.Group_ID,
		a.Participant_ID,
		a.Contact_ID,
		a.Nickname,
		a.Last_Name
END

DECLARE @IDs TABLE (ID INT)
-- add the proc and permissions to the mp db for 'api_crds_Get_Group_Roles_By_Event'
IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_Get_Group_Roles_By_Event')
BEGIN	

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	OUTPUT INSERTED.API_Procedure_ID INTO @IDs
	VALUES('api_crds_Get_Group_Roles_By_Event')

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
