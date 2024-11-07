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

        public void InsertarDetalleVenta(entDetalleVenta detalle)
        {
            try
            {
                datDetalleVenta.Instancia.InsertarDetalleVenta(detalle);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<entDetalleVenta> ListarDetallesPorVenta(int idVenta)
        {
            return datDetalleVenta.Instancia.ListarDetallesPorVenta(idVenta);
        }
        public List<entDetalleVenta> MostrarDetallesVenta(int idVenta)
        {
            return datDetalleVenta.Instancia.MostrarDetallesVenta(idVenta);
        }
        public void EliminarDetalleVenta(int idDetalle)
        {
            try
            {
                datDetalleVenta.Instancia.EliminarDetalleVenta(idDetalle);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void ModificarCantidadDetalle(int idDetalle, int cantidad, double precio)
        {
            try
            {
                double subtotal = precio * cantidad;
                datDetalleVenta.Instancia.ModificarCantidadDetalle(idDetalle, cantidad, subtotal);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
    }
}
