CREATE PROCEDURE Auth.sp_inactivateUserControlUsuarios   
    @Id int
AS   
    SET NOCOUNT ON;  
    UPDATE Auth.Users set status = 0 where id = @Id
GO