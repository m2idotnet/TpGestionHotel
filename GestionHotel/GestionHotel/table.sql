﻿CREATE TABLE [dbo].[Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [firstName] VARCHAR(50) NOT NULL, 
    [lastName] VARCHAR(50) NOT NULL, 
    [phone] VARCHAR(50) NOT NULL, 
    [address] VARCHAR(MAX) NOT NULL,
[hotelId] INT NOT NULL, 
)
CREATE TABLE [dbo].[Booking]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Code] VARCHAR(50) NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [RoomId] INT NOT NULL, 
    [OccupatedNumber] INT NOT NULL, 
    [status] INT NOT NULL,
	[invoiceStatus] INT NOT NULL DEFAULT 1, 
)
CREATE TABLE [dbo].[Hotel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] VARCHAR(50) NOT NULL, 
    [RoomsNumber] INT NOT NULL
)
CREATE TABLE [dbo].[Room]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Number] INT NOT NULL, 
    [HotelId] INT NOT NULL, 
    [OccupatedMax] INT NOT NULL,
	[price] DECIMAL NOT NULL, 
[Status] INT NULL, 
)
CREATE TABLE [dbo].[invoice] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CustomerId] INT          NOT NULL,
    [Price]      DECIMAL (18) NOT NULL,
    [Status] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

