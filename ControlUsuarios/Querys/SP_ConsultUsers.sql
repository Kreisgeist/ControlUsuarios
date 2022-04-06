CREATE PROCEDURE Auth.sp_consultUserControlUsuarios   
AS   
    SET NOCOUNT ON;  
    SELECT * FROM Auth.Users
GO