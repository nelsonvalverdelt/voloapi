CREATE TABLE [dbo].[Airport]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Code] VARCHAR(35) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [CoordinateId] INT NOT NULL, 
    [CityId] INT NOT NULL, 
    CONSTRAINT [FK_Airport_Coordinate] FOREIGN KEY (CoordinateId) REFERENCES Coordinate(Id), 
    CONSTRAINT [FK_Airport_City] FOREIGN KEY (CityId) REFERENCES City(Id)
)
