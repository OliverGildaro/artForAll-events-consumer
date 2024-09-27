ALTER PROCEDURE getRoomsOfBuildings
	@bldgCode as char(10)
AS
BEGIN
SET NOCOUNT ON
select *
	from fmb0 as b
	INNER JOIN fma0 as a 
		ON b.BLDGCODE = a.BLDGCODE
		WHERE b.BLDGCODE = @bldgCode
END

EXECUTE getRoomsOfBuildings @bldgCode = 'NA01'
--DROP PROCEDURE getRoomsOfBuildings
--ALTER PROCEDURE getRoomsOfBuildings

--INPUT - OUTPUT
CREATE PROCEDURE getRoomVertices
	@rmId as char(16),
	@vertices nvarchar(max) OUTPUT
AS
BEGIN
SET NOCOUNT ON
select @vertices = a.VERTICES
	from fma0 as a 
		WHERE a.RMID = @rmId
END

DECLARE @vertices nvarchar(max)
EXECUTE getRoomVertices @rmId = '91-318',
						@vertices = @vertices OUTPUT
PRINT(@vertices)
EXECUTE sp_helptext 'getRoomVertices'


--ENCRIPTION on SP
CREATE PROCEDURE getRoomVerticesEncripted
	@rmId as char(16)
WITH ENCRYPTION
AS
BEGIN
SET NOCOUNT ON
select *
	from fma0 as a 
		WHERE a.RMID = @rmId
END

EXECUTE getRoomVerticesEncripted @rmId = '91-318'
EXECUTE sp_helptext 'getRoomVerticesEncripted'


--RENAME SP
CReATE PROCEDURE getRoomsOfBuildings2
	@bldgCode as char(10)
AS
BEGIN
SET NOCOUNT ON
select *
	from fmb0 as b
	INNER JOIN fma0 as a 
		ON b.BLDGCODE = a.BLDGCODE
		WHERE b.BLDGCODE = @bldgCode
END
EXECUTE sp_rename 'getRoomsOfBuildings2', 'getRoomsOfBuildings2Renamed'

