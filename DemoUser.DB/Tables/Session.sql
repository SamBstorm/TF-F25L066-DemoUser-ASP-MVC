CREATE TABLE [dbo].[Session]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Session PRIMARY KEY DEFAULT NEWID(),
    
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Token] UNIQUEIDENTIFIER NOT NULL CONSTRAINT UQ_Session_Token UNIQUE,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [ExpiresAt] DATETIME2 NOT NULL,
    [RevokedAt] DATETIME2 NULL,

    CONSTRAINT FK_Session_User FOREIGN KEY (UserId)
    REFERENCES [dbo].[User] ([Id])
    );

-- User 1 - n Session & chaque session a un Token (GUID) & ExpiresAt + RevokedAt = gestion simple d’expiration.