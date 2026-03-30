CREATE PROCEDURE ObtenerSubCategoriasPorCategoria
    @CategoriaId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT Id, IdCategoria, Nombre
    FROM dbo.SubCategorias
    WHERE IdCategoria = @CategoriaId;
END