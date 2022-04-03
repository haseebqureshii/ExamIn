CREATE PROCEDURE [dbo].[spUserImageSave]
	@Id nvarchar(128),
	@file varbinary(max)
AS
begin
	set nocount on;

	SELECT * FROM [dbo].[User]
	UPDATE [dbo].[User]
    SET FacePrint = @file
    WHERE Id = @Id
end
