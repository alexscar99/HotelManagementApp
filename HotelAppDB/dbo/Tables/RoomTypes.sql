﻿CREATE TABLE [dbo].[RoomTypes]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(2000) NOT NULL, 
    [Price] MONEY NOT NULL
)