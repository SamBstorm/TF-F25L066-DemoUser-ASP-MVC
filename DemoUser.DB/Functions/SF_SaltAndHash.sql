CREATE FUNCTION [dbo].[SF_SaltAndHash]
(
	@password NVARCHAR(64),
	@salt UNIQUEIDENTIFIER --############-####-####-####-#########
)
RETURNS VARBINARY(32)
AS
BEGIN
	DECLARE @saltedPassword NVARCHAR(100)
	SET @saltedPassword = CONCAT(@salt, @password);

	RETURN HASHBYTES('SHA2_256', @saltedPassword)
END
