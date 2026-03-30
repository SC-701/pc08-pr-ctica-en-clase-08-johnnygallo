CREATE PROCEDURE ObtenerProducto
	-- Add the parameters for the stored procedure here
    @Id uniqueidentifier
AS
BEGIN
	
SELECT        Producto.Id, Producto.IdSubCategoria, Producto.Nombre, Producto.Descripcion, Producto.Precio, Producto.Stock, Producto.CodigoBarras, SubCategorias.Nombre AS Expr1, Categorias.Nombre AS Expr2
FROM            Producto INNER JOIN
                         SubCategorias ON Producto.IdSubCategoria = SubCategorias.Id INNER JOIN
                         Categorias ON SubCategorias.IdCategoria = Categorias.Id
WHERE        (Producto.Id = @Id)
END