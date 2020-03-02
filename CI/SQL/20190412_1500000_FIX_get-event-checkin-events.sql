USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_Get_Event_Checkin_Events]    Script Date: 4/12/2019 1:05:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Brian Cavanaugh (Callibrity)
-- Create date: 12/05/2018
-- Description:	Retrieve event checkin events
-- for a specific contact and a given date
-- Modified: 1/10/2019 - Added criteria to select events 
-- that are Cancelled = 0 and [Allow_Check-in] = 0
-- Fixed issue with group information showing up if there are multiple groups
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[api_crds_Get_Event_Checkin_Events]
	@ContactId INT,
	@EventDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	-- Only use for testing purposes
	--DECLARE	@ContactId INT = 7754510
	--DECLARE @EventDate DATETIME = '2018-12-05'

	-- Set Group Role IDs for Leader and Member
	DECLARE @Group_Leader_ID INT = 22
	DECLARE @Group_Member_ID INT = 16
	
	DECLARE @Events TABLE
	(
		Event_Id INT,
		Event_Title VARCHAR(75),
		Event_Start_Date DATETIME,
		Event_End_Date DATETIME,
		Congregation_Name VARCHAR(50),
		GroupCount INT
	)
	
	INSERT INTO @Events (Event_Id, Event_Title, Event_Start_Date, Event_End_Date, GroupCount, Congregation_Name)
	SELECT DISTINCT e.Event_ID, e.Event_Title, e.Event_Start_Date, e.Event_End_Date, COUNT(eg2.Group_ID), c.Congregation_Name--, g.Group_ID, g.Group_Name
	FROM Groups g
		JOIN Group_Participants gp on gp.Group_ID = g.Group_ID
		JOIN Participants p on p.Participant_ID = gp.Participant_ID
		JOIN Event_Groups eg on eg.Group_ID = g.Group_ID
		JOIN Events e on e.Event_ID = eg.Event_ID
		JOIN Event_Groups eg2 on eg2.Event_ID = e.Event_ID
		JOIN Congregations c on c.Congregation_ID = e.Congregation_ID
	WHERE p.Contact_ID = @ContactId
		AND gp.Group_Role_ID IN (@Group_Leader_ID, @Group_Member_ID)
		AND (gp.End_Date IS NULL OR gp.End_Date >= @EventDate)
		AND e.Event_Start_Date BETWEEN @EventDate AND DATEADD(DAY, 1, @EventDate)
		AND g.Event_CheckIn = 1
		AND e.Cancelled = 0
		AND e.[Allow_Check-in] = 0
	GROUP BY e.Event_ID, e.Event_Title, e.Event_Start_Date, e.Event_End_Date, c.Congregation_Name

	SELECT DISTINCT e.Event_Id, e.Event_Title, e.Event_Start_Date, e.Event_End_Date, e.Congregation_Name,
		CASE e.GroupCount WHEN 1 THEN g.Group_ID ELSE null END AS Group_Id,
		CASE e.GroupCount WHEN 1 THEN g.Group_Name ELSE null END AS Group_Name
	FROM @Events e
		JOIN Event_Groups eg ON eg.Event_ID = e.Event_Id
		JOIN Groups g ON g.Group_ID = eg.Group_ID

END

GO


