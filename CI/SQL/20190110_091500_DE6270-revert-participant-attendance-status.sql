USE [MinistryPlatform]
GO

/****** Object: SqlProcedure [dbo].[api_crds_Revert_Participant_Attendance_Status] Script Date: 1/3/2019 11:17:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Brian Cavanaugh (Callibrity)
-- Create date: 12/26/2018
-- Description:	Revert the participation status
-- of an event participant to the previous value
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[api_crds_Revert_Participant_Attendance_Status]
	@EventParticipantId INT
AS
BEGIN
	SET NOCOUNT ON;

	-- Search for the Event_Participant record in the audit log
	DECLARE @AuditItemID INT = 0
	SELECT TOP 1 @AuditItemID = Audit_Item_ID
	FROM dp_Audit_Log
	WHERE Table_Name = 'Event_Participants'
		AND Record_ID = @EventParticipantId
	ORDER BY Date_Time DESC
	
	-- Search for the Audit Item Id in the audit details
	DECLARE @Previous_ID INT = 0
	SELECT TOP 1 @Previous_ID = Previous_ID
	FROM dp_Audit_Detail
	WHERE Audit_Item_ID = @AuditItemID
		AND Field_Name = 'Participation_Status_ID'
	ORDER BY Audit_Detail_ID DESC

	IF @Previous_ID = 0
	BEGIN
		-- If Previous_ID field is 0, delete the event participant record
		DELETE Event_Participants
		WHERE Event_Participant_ID = @EventParticipantId
		SELECT @Previous_ID as 'ReturnId'
	END
	ELSE
	BEGIN
		-- If Previous_ID field is not 0, update the Event_Participant record (Participation_Status_ID field) with that value
		UPDATE Event_Participants
		SET Participation_Status_ID = @Previous_ID
		WHERE Event_Participant_ID = @EventParticipantId
		SELECT @EventParticipantId as 'ReturnId'
	END
END
GO

-- add the proc and permissions to the mp db for 'api_crds_Revert_Participant_Attendance_Status'
IF NOT EXISTS(SELECT * FROM [dbo].[dp_API_Procedures] WHERE [procedure_name] = 'api_crds_Revert_Participant_Attendance_Status')
BEGIN
	DECLARE @IDs TABLE (ID INT)

	INSERT INTO [dbo].[dp_API_Procedures] ([procedure_name]) 
	OUTPUT INSERTED.API_Procedure_ID INTO @IDs
	VALUES('api_crds_Revert_Participant_Attendance_Status')

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
