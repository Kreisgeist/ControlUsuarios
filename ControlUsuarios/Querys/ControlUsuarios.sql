USE ControlUsuarios;

CREATE SCHEMA Auth Authorization dbo;

CREATE SCHEMA Catalogs Authorization dbo;

CREATE TABLE Catalogs.Genders (

	id int identity primary key not null,
	Gender nvarchar(25) not null

)

INSERT INTO Catalogs.Genders VALUES ('Masculino')
INSERT INTO Catalogs.Genders VALUES ('Femenino')

CREATE TABLE Auth.Users (

	id int identity primary key not null,
	[user] nvarchar(25) not null,
	[password] nvarchar(50) not null,
	email nvarchar(100) not null,
	gender int not null,
	constraint fk_CatalogsGender_AuthUsers
	foreign key(gender) references Catalogs.Genders(id),
	creationDate datetime2 default getdate() not null,
	[status] bit not null

)