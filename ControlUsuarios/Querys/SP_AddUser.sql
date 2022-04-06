CREATE PROCEDURE Auth.sp_addUserControlUsuarios   
    @User nvarchar(25),   
    @Password nvarchar(50),
	@Email nvarchar(100),
	@Gender int
AS   
    SET NOCOUNT ON;  
    INSERT INTO Auth.Users ([user], [password], email, gender, [status])
	VALUES (@User, @Password, @Email, @Gender, 1);
GO