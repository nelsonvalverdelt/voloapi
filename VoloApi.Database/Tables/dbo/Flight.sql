CREATE TABLE [dbo].[Flight]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [HostId] NVARCHAR(128) NOT NULL, 
    [Capacity] INT NOT NULL, 
    [CrewId] NVARCHAR(128) NOT NULL, 
    [PlaneImage] NVARCHAR(180) NOT NULL, 
    [Budget] DECIMAL(7, 2) NOT NULL, 
    [DepartureAirportId] INT NOT NULL, 
    [ArrivalAirportId] INT NOT NULL, 
    [DepartureDate] DATETIME NOT NULL, 
    [ArrivalDate] DATETIME NOT NULL, 
    [CostPerPerson] DECIMAL(7, 2) NOT NULL, 
    CONSTRAINT [FK_Flight_User_Host] FOREIGN KEY (HostId) REFERENCES [dbo].[User](Id), 
    CONSTRAINT [FK_Flight_User_Crew] FOREIGN KEY (CrewId) REFERENCES [dbo].[User](Id), 
    CONSTRAINT [FK_Flight_Airport_Departure] FOREIGN KEY (DepartureAirportId) REFERENCES Airport(Id), 
    CONSTRAINT [FK_Flight_Airtport_Arrival] FOREIGN KEY (ArrivalAirportId) REFERENCES Airport(Id)
)
