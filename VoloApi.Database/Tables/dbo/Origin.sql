CREATE TABLE [dbo].[Origin]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [AirportId] INT NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [Time] TIMESTAMP NOT NULL, 
    CONSTRAINT [FK_Origin_Airport] FOREIGN KEY (AirportId) REFERENCES Airport(Id)
)
