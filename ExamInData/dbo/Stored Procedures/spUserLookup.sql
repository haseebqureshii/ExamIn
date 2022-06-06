CREATE PROCEDURE [dbo].[spUserLookup]
	@_id nvarchar(128)
AS
begin
	set nocount on;
	
	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate
	from [dbo].[User]
	where Id = @_id;
end
