CREATE PROCEDURE EditarProducto
    @Id AS uniqueidentifier
    ,@IdSubCategoria AS uniqueidentifier
    ,@Nombre AS varchar(max)
    ,@Descripcion AS varchar(max)
    ,@Precio AS decimal(18,0)
    ,@Stock AS int
    ,@CodigoBarras AS varchar(max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
UPDATE [dbo].[Producto]
   SET 
       [IdSubCategoria] = @IdSubCategoria
      ,[Nombre] = @Nombre
      ,[Descripcion] = @Descripcion
      ,[Precio] = @Precio
      ,[Stock] = @Stock
      ,[CodigoBarras] = @CodigoBarras
 WHERE Id = @Id
END