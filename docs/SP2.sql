--RENAME SP
ALTER PROCEDURE insertInEvents
	@flag bit OUTPUT, --return 1 success, return 0 fails
	@Id as nvarchar(35),
	@Name as nvarchar(100),
	@Description as nvarchar(1000),
	@Date as datetime,
	@Type as nvarchar(30),
	@State as nvarchar(10)
AS
BEGIN
BEGIN TRANSACTION
BEGIN TRY
SET NOCOUNT ON
INSERT INTO dbo.Events
(id,Name,Description,Date,CreatedAt,Type,State) VALUES
(@Id,@Name,@Description,@Date,GETDATE(),@Type,@State)
SET @flag=1
IF @@TRANCOUNT > 0
BEGIN COMMIT TRANSACTION;
END
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
BEGIN ROLLBACK TRANSACTION;
END
SET @flag = 0;
END CATCH
END

DECLARE @flag bit
EXECUTE insertInEvents
@flag output,
@Id='51aafgca-3gg0-4143-9b6f-hyb607837390',
@Name='New name22',
@Description ='Descr event',
@Date='2024-09-24 16:16:25.313',
@Type='Music',
@State='DRAFT'

if @flag = 1
PRINT 'success'
else
PRINT 'ERROR'

/*Create a new role for Sales Manager*/
CREATE ROLE EventsManager;
GO
GRANT EXECUTE ON SCHEMA::Sales TO EventsManager
GO
GRANT SELECT ON SCHEMA::Sales TO EventsManager
GO
CREATE USER DemoUser WITHOUT LOGIN;
GO
ALTER ROLE EventsManager
ADD MEMBER DemoUser;
GO
EXECUTE AS USER = 'DemoUser';
GO
/*Insert rows directly into the table*/

