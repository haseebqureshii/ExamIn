CREATE PROCEDURE [dbo].[spUserAdd]
	@id nvarchar(128),
	@firstName nvarchar(50),
	@lastName nvarchar(50),
	@email nvarchar(256),
	@createdDate Datetime2(7)
AS

begin
	set nocount on;

	INSERT INTO [dbo].[User] (
		ID, 
		FirstName, 
		LastName, 
		EmailAddress, 
		CreatedDate
	)
	VALUES (
		@id, 
		@firstName, 
		@lastName, 
		@email, 
		@createdDate
	);
end
