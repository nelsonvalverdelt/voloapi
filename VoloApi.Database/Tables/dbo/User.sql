CREATE TABLE [dbo].[User](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Firstname] NVARCHAR(65) NOT NULL, 
    [MiddleName] NVARCHAR(65) NOT NULL, 
    [LastName] NVARCHAR(65) NOT NULL, 
    [ImageUrl] NVARCHAR(180) NULL, 
    [Address] NVARCHAR(120) NULL, 
    [City] NVARCHAR(120) NULL, 
    [ZipCode] NVARCHAR(12) NULL, 
    [State] NVARCHAR(120) NULL, 
    [UserType] VARCHAR(25) NOT NULL, 
    [FullName] NVARCHAR(254) NOT NULL, 
    CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO