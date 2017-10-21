CREATE TABLE [dbo].[HelperEvent]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [DepartureDay] VARCHAR(10) NOT NULL, 
    [DepartureNumberDay] TINYINT NOT NULL, 
    [DepartureMonth] VARCHAR(10) NOT NULL, 
    [DepartureAirport] TINYINT NOT NULL, 
    [ReturnDay] VARCHAR(10) NOT NULL, 
    [ReturnNumberDay] TINYINT NOT NULL, 
    [ReturnMonth] VARCHAR(10) NOT NULL, 
    [ReturnAirport] NVARCHAR(50) NOT NULL, 
    [ReturnTime] VARCHAR(26) NOT NULL
)
