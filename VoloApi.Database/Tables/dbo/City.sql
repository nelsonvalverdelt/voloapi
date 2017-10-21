CREATE TABLE [dbo].[City]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL, 
    [StateId] INT NOT NULL, 
    CONSTRAINT [FK_City_State] FOREIGN KEY (StateId) REFERENCES [dbo].[State](Id)
)
