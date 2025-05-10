CREATE TABLE [dbo].[ServiceRequest] (
    [RequestId]     INT           IDENTITY (1, 1) NOT NULL,
    [UserID]        INT           NOT NULL,
    [ReqTitle]      NVARCHAR (50) NOT NULL,
    [Location]      NVARCHAR (15) NOT NULL,
    [StreetName]    NVARCHAR (30) NOT NULL,
    [PriorityLevel] NVARCHAR (30) NOT NULL,
    [Category]      NVARCHAR (20) NOT NULL,
    [Description]   TEXT          NULL,
    [Status]        NVARCHAR (20) NULL,
    [DateSubmitted] NVARCHAR (20) NULL,
    CONSTRAINT [PK_ServiceRequest] PRIMARY KEY CLUSTERED ([RequestId] ASC)
);