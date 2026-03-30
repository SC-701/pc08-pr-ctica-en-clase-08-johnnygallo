CREATE PROCEDURE EliminarProducto
    @Id AS uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DELETE
FROM Producto
WHERE        (Id = @Id)
END