using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logDetalleVenta
    {
        private static readonly logDetalleVenta _instancia = new logDetalleVenta();
        public static logDetalleVenta Instancia
        {
            get
            {
                return logDetalleVenta._instancia;
            }
        }

        public List<entDetalleVenta> InsertarDetalleVenta(entDetalleVenta detalle)
        {
            // Llamamos al método de la capa de datos y retornamos la lista de detalles
            return datDetalleVenta.Instancia.InsertarDetalleVenta(detalle);
        }
    }
}
