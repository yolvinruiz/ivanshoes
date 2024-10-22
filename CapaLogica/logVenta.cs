using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logVenta
    {
        public static readonly logVenta _instancia = new logVenta();
        public static logVenta Instancia
        {
            get
            {
                return logVenta._instancia;
            }
        }
        public int InsertarVenta(entVenta venta)
        {
            return datVenta.Instancia.InsertarVenta(venta);
        }
    }
}
