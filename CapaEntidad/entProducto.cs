using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entProducto
    {
        public int id_producto { get; set; }
        public string nombre { get; set; }
        public double precio { get; set; }
        public int stock { get; set; }
        public int id_tipo_producto { get; set; }
        public int id_marca { get; set; }
        public int id_talla { get; set; }
        public int id_color { get; set; }
        public int id_categoria { get; set; }
        public string NombreTalla { get; set; }
        public string NombreTipoProducto { get; set; }
        public string NombreMarca { get; set; }
        public string NombreColor { get; set; }
        public string NombreCategoria { get; set; }
        public string Imagen { get; set; }
    }
}
