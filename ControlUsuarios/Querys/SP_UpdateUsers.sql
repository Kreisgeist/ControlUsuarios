CREATE PROCEDURE Auth.sp_updateUserControlUsuarios   
    @Id int,
	@User nvarchar(25),
	@Password nvarchar(50),
	@Email nvarchar(100),
	@Gender int
AS   
    SET NOCOUNT ON;  
    UPDATE Auth.Users set [user] = @User, [password] = @Password, email = @Email, gender = @Gender where id = @Id
GO