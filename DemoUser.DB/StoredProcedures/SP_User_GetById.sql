CREATE PROCEDURE [dbo].[SP_User_GetById]
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Email], [CreatedAt], [DisabledAt]
	FROM [User]
	WHERE [Id] = @Id;
END