USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_Get_Attendance_Summary]    Script Date: 11/16/2018 9:26:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Brian Cavanaugh (Callibrity)
-- Create date: 02/18/2019
-- Description:	Get Attendance Summary Report
-- for a given Event ID 
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[api_crds_Get_Attendance_Summary]
	@EventId INT
AS
BEGIN
	SET NOCOUNT ON;
	--SET @EventId = 4609522

-- Get list of Group IDs for a given Event ID
DECLARE @Groups TABLE
(
	GroupID INT
) 

INSERT INTO @Groups
SELECT DISTINCT Group_ID
	FROM Event_Groups
	WHERE Event_ID = @EventId

-- Get list of Event IDs for the determined Group IDs and if the Event Start Date is before or equal to GETDATE()
DECLARE @Events TABLE
(
	EventID INT,
	EventStartDate DATETIME
)

INSERT INTO @Events
SELECT DISTINCT eg.Event_ID, e.Event_Start_Date
	FROM Event_Groups eg
		JOIN [Events] e ON eg.Event_ID = e.Event_ID
	WHERE Group_ID in (SELECT GroupID FROM @Groups)
		AND e.Event_Start_Date <= GETDATE()
	ORDER BY e.Event_Start_Date

-- Get list of Group Participants for the determined Group IDs
DECLARE @GroupParticipants TABLE
(
	GroupParticipantID INT,
	ParticipantID INT,
	Nickname VARCHAR(50),
	LastName VARCHAR(50)
)

INSERT INTO @GroupParticipants
SELECT gp.Group_Participant_ID, gp.Participant_ID, c.Nickname, c.Last_Name
FROM Group_Participants gp
	JOIN Participants p ON gp.Participant_ID = p.Participant_ID
	JOIN Contacts c ON p.Contact_ID = c.Contact_ID
WHERE gp.Group_ID IN (SELECT GroupID FROM @Groups)

-- Get list of Event Participants for each of the determined Event IDs and Group Participants
DECLARE @EventParticipants TABLE
(
	EventParticipantID INT,
	EventID INT,
	ParticipantID INT,
	ParticipationStatusID INT
)

INSERT INTO @EventParticipants
SELECT ep.Event_Participant_ID, ep.Event_ID, ep.Participant_ID, ep.Participation_Status_ID
FROM Event_Participants ep
WHERE ep.Participant_ID IN (SELECT ParticipantID FROM @GroupParticipants)
	and ep.Event_ID IN (SELECT EventID FROM @Events)
	
------------------------------------------------
--select * from @Groups
--select * from @GroupParticipants
--select * from @Events
--select * from @EventParticipants

------------------------------------------------
DECLARE @TotalEvents INT = 0;
SELECT @TotalEvents = COUNT(*) FROM @Events

SELECT gp.ParticipantID
	,gp.Nickname
	,gp.LastName
	,la.LastAttended
	,CASE 
		WHEN la.LastAttended IS NULL THEN @TotalEvents
		ELSE la.MissedTotal
		END AS MissedTotal
	,CASE 
		WHEN la.LastAttended IS NULL THEN @TotalEvents
		ELSE (SELECT COUNT(*) FROM @Events sub_e WHERE sub_e.EventStartDate > la.LastAttended) 
		END AS MissedConsecutive
FROM @GroupParticipants gp
	LEFT OUTER JOIN (SELECT ep.ParticipantID, MAX(e.EventStartDate) AS LastAttended, (@TotalEvents - count(*)) AS MissedTotal
						FROM @EventParticipants ep 
							JOIN @Events e ON ep.EventID = e.EventID 
						WHERE ep.ParticipationStatusID = 3
						GROUP BY ep.ParticipantID) la ON la.ParticipantID = gp.ParticipantID
END

DECLARE @IDs TABLE (ID INT)
-- add the proc and permissions to the mp db for 'api_crds_Get_Attendance_Summary'
IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_Get_Attendance_Summary')
BEGIN	

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	OUTPUT INSERTED.API_Procedure_ID INTO @IDs
	VALUES('api_crds_Get_Attendance_Summary')

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
