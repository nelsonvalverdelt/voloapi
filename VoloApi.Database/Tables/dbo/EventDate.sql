/*
CREATE TABLE [dbo].[EventDate]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [DepartureDate] DATETIME NOT NULL, 
    [DepartureTime] TIMESTAMP NOT NULL, 
    [ArrivalDate] DATETIME NOT NULL, 
    [ArrivalTime] TIMESTAMP NOT NULL, 
    [ReturnDepartureDate] DATETIME NOT NULL, 
    [ReturnDepartureTime] TIMESTAMP NOT NULL, 
    [ReturnArrivalDate] DATETIME NOT NULL, 
    [ReturnArrivalTime] VARCHAR(10) NOT NULL
)
*/