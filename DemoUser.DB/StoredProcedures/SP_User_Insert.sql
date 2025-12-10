CREATE PROCEDURE [dbo].[SP_User_Insert]
	@email NVARCHAR(320),
	@password NVARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @salt UNIQUEIDENTIFIER = NEWID()

	INSERT INTO [User]([Email],[Password],[Salt])
		OUTPUT [inserted].[Id]
		VALUES (@email, [dbo].[SF_SaltAndHash](@password, @salt), @salt)
END