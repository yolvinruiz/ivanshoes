using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entDetalleVenta
    {
        public int ID_Detalle_venta { get; set; }
        public int id_Venta { get; set; }
        public int id_Producto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public double Preciounitario { get; set; }
        public double Subtotal { get; set; }

    }
}
