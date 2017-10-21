CREATE TABLE [dbo].[Event]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [DepartureId] NVARCHAR(128) NOT NULL, 
    [ReturnId] NVARCHAR(128) NOT NULL, 
    [HostId] NVARCHAR(128) NOT NULL, 
    [TailNumbers] INT NOT NULL, 
    [NumberOfSeats] INT NOT NULL, 
    [ApproximateHourlyRate] INT NOT NULL, 
    [ApproximateFlightTime] INT NOT NULL, 
    [FullCapacityPrice] DECIMAL(7, 2) NOT NULL, 
    [CurrentCapacityPrice] DECIMAL(7, 2) NOT NULL, 
    [IsPrivate] BIT NOT NULL, 
    [AirplanePhotoUrl] NVARCHAR(180) NOT NULL, 
    [TotalPrice] DECIMAL(7, 2) NOT NULL, 
    [UserId] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [FK_Event_Flight_Departure] FOREIGN KEY (DepartureId) REFERENCES Flight(Id), 
    CONSTRAINT [FK_Event_Flight_Return] FOREIGN KEY (ReturnId) REFERENCES Flight(Id), 
    CONSTRAINT [FK_Event_User_Host] FOREIGN KEY (HostId) REFERENCES [dbo].[User](Id), 
    CONSTRAINT [FK_Event_Users] FOREIGN KEY (UserId) REFERENCES [dbo].[User](Id)
)
