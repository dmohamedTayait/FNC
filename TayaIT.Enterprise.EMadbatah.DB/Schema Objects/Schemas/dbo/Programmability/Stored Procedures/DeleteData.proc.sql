-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DeleteData
	
AS
BEGIN
	
	SET NOCOUNT ON;
delete from sessioncontentitem
delete from SessionAttendant
delete from attendant
delete from agendasubitem
delete from agendaitem
delete from Attachement
delete from sessionfile
delete from sessionstart
delete from [session]
    
END
