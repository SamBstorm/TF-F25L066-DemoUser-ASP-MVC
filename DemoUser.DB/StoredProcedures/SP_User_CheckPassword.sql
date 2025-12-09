CREATE PROCEDURE [dbo].[SP_User_CheckPassword]
	@email NVARCHAR(320),
	@password NVARCHAR(64)
AS
BEGIN
	SELECT [Id]
		FROM [User]
		WHERE	[Email] = @email
			AND	[Password] = [dbo].[SF_SaltAndHash](@password, [Salt])
			AND ([DisabledAt] IS NULL OR GETDATE() < DATEADD(DAY, 30,[DisabledAt]))
END