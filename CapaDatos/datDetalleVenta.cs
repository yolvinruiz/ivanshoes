using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datDetalleVenta
    {
        public static readonly datDetalleVenta _instancia = new datDetalleVenta();
        public static datDetalleVenta Instancia
        {
            get
            {
                return datDetalleVenta._instancia;
            }
        }

        public List<entDetalleVenta> InsertarDetalleVenta(entDetalleVenta detalle)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<entDetalleVenta> listaDetalles = new List<entDetalleVenta>();

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("InsertarDetalleVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_Venta", detalle.id_Venta);
                cmd.Parameters.AddWithValue("@id_Producto", detalle.id_Producto);
                cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                cmd.Parameters.AddWithValue("@Preciounitario", detalle.Preciounitario);
                cmd.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);

                cn.Open();
                dr = cmd.ExecuteReader();

                // Leer los resultados de la consulta
                while (dr.Read())
                {
                    entDetalleVenta nuevoDetalle = new entDetalleVenta
                    {
                        id_Venta = Convert.ToInt32(dr["id_Venta"]),
                        NombreProducto = dr["nombre"].ToString(),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Preciounitario = Convert.ToDouble(dr["Preciounitario"]),
                        Subtotal = Convert.ToDouble(dr["Subtotal"])
                    };

                    listaDetalles.Add(nuevoDetalle);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                    cmd.Connection.Close();
            }

            return listaDetalles;
        }
    }
}
