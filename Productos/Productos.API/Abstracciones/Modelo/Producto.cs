using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelo
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "La propiedad Nombre es requerida")]
        [StringLength(15, MinimumLength = 2,
        ErrorMessage = "El Nombre debe tener entre 2 y 15 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La propiedad Descripcion es requerida")]
        [StringLength(40, MinimumLength = 4,
            ErrorMessage = "La propiedad color debe ser mayor a 4 caracteres y menos a 40")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La propiedad Precio es requerida")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La propiedad Stock es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La propiedad Codigo de Barras es requerida")]
        [StringLength(13, MinimumLength = 8,
        ErrorMessage = "El código de barras debe tener entre 8 y 13 caracteres")]
        public string CodigoBarras { get; set; }
    }

    public class ProductoRequest : ProductoBase
    {
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        public Guid Id { get; set; }
        public string? SubCategoria { get; set; }
        public string? Categoria { get; set; }
    }

    public class ProductoDetalle : ProductoResponse
    {
        public decimal PrecioDolar { get; set; }
        public decimal PrecioEnDolar { get; set; }
    }
}
