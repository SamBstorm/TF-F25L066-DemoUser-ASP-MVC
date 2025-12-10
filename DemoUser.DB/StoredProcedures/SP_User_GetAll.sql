CREATE PROCEDURE [dbo].[SP_User_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Email], [CreatedAt], [DisabledAt]
	FROM [User]
END
RETURN 0
