USE [MinistryPlatform]
GO

/****** Object: SqlProcedure [dbo].[api_crds_Get_Group_Event_Participants] Script Date: 12/28/2018 11:47:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      John Cleaver
-- Create date: 2018-12-10
-- Description:	Create api_crds_Get_Group_Event_Participants to get Group Participants
-- and matching Event Participant Records
-- Modified: 12/28/18 (Brian Cavanaugh) added a "TOP 1" to subqueries to guard against errors
-- in case data is not setup correctly
-- Modified: 1/10/2019 (Brian Cavanaugh) added End_Date is null to select only Group_Participants
-- that are active.
--
-- Notes: This looks for a person to have a group participant record on the specified group,
-- regardless of role. The event participant id being returned is assumed to be in the context
-- of the member event participant record if they have more than one event participant record. 
-- If they are a leader on the group only, the EP record will be for their leader record.
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[api_crds_Get_Group_Event_Participants]
	@EventId INT,
	@GroupId INT
AS
BEGIN

DECLARE @tblRawParticipants AS TABLE
(
	Group_Participant_ID INT,
	g_ID INT,
	p_ID INT,
	Contact_ID INT,
	Last_Name NVARCHAR(50),
	Nickname NVARCHAR(50),
	Is_Leader BIT,
	Event_Participant_ID INT,
	Participation_Status_ID INT
)

-- insert member records a raw list of participants in that group, regardless of their roles
INSERT INTO @tblRawParticipants
SELECT DISTINCT
	NULL, --Group_Participant_ID,
	g.Group_ID,
	p.Participant_ID,
	c.Contact_ID,
	Last_Name, 
	Nickname,
	0,
	(SELECT TOP 1 Event_Participant_ID FROM Event_Participants ep WHERE ep.Event_ID = @EventId AND ep.Participant_ID = p.Participant_ID) AS 'Event_Participant_ID',
	(SELECT TOP 1 Participation_Status_ID FROM Event_Participants ep WHERE ep.Event_ID = @EventId AND ep.Participant_ID = p.Participant_ID) AS 'Participation_Status_ID'   
FROM Events e INNER JOIN Event_Groups eg ON e.Event_ID = eg.Event_ID
INNER JOIN Groups g ON eg.Group_ID = g.Group_ID
INNER JOIN Group_Participants gp ON gp.Group_ID = g.Group_ID
INNER JOIN Participants p ON p.Participant_ID = gp.Participant_ID
INNER JOIN Contacts c ON c.Contact_ID = p.Contact_ID
WHERE
e.Event_ID = @EventId
AND eg.Group_ID = @GroupId
AND (gp.End_Date is null OR gp.End_Date > GETDATE())

-- get the group participant for each record above, ordering by member role first
UPDATE @tblRawParticipants SET Group_Participant_ID = (SELECT TOP(1) Group_Participant_ID
	FROM Group_Participants gp
	WHERE gp.Group_ID = @GroupId AND gp.Participant_ID = p_ID
	ORDER BY Group_Role_ID ASC)

-- check to see if the person is a leader
UPDATE @tblRawParticipants SET Is_Leader = 
IIF((SELECT COUNT(*) FROM Group_Participants sub_gp WHERE sub_gp.Participant_ID = p_ID 
AND sub_gp.Group_ID = @GroupId AND sub_gp.Group_Role_ID = 22) > 0, 1, 0)

-- select the data with the API-friendly column names
SELECT
	Group_Participant_ID,
	g_ID AS Group_ID,
	p_ID AS Participant_ID,
	Contact_ID,
	Last_Name,
	Nickname,
	Is_Leader,
	Event_Participant_ID,
	Participation_Status_ID
FROM @tblRawParticipants

END

-- add the proc and permissions to the mp db
IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_Get_Group_Event_Participants')
BEGIN
	DECLARE @IDs TABLE (ID INT)

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	OUTPUT INSERTED.API_Procedure_ID INTO @IDs
	VALUES('api_crds_Get_Group_Event_Participants')

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
