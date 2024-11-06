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

       
        public List<entDetalleVenta> ListarDetallesPorVenta(int idVenta)
        {
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            List<entDetalleVenta> listaDetalles = new List<entDetalleVenta>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerDetallesVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_Venta", idVenta);

                cn.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    entDetalleVenta detalle = new entDetalleVenta
                    {
                        ID_Detalle_venta = Convert.ToInt32(dr["ID_Detalle_venta"]),
                        id_Venta = Convert.ToInt32(dr["id_Venta"]),
                        id_Producto = Convert.ToInt32(dr["id_Producto"]),
                        NombreProducto = dr["NombreProducto"].ToString(),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Preciounitario = Convert.ToDouble(dr["Preciounitario"]),
                        Subtotal = Convert.ToDouble(dr["Subtotal"])
                    };
                    listaDetalles.Add(detalle);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return listaDetalles;
        }
        public List<entDetalleVenta> MostrarDetallesVenta(int idVenta)
        {
            SqlCommand cmd = null;
            List<entDetalleVenta> lista = new List<entDetalleVenta>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("MostrarDetallesVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_Venta", idVenta);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    entDetalleVenta detalleVenta = new entDetalleVenta
                    {
                        ID_Detalle_venta = Convert.ToInt32(dr["ID_Detalle_venta"]),
                        id_Venta = Convert.ToInt32(dr["id_Venta"]),
                        id_Producto = Convert.ToInt32(dr["id_Producto"]),
                        NombreProducto = dr["NombreProducto"].ToString(),
                        NombreTalla = dr["NombreTalla"].ToString(),
                        Imagen = dr["Imagen"].ToString(),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Preciounitario = Convert.ToDouble(dr["Preciounitario"]),
                        Subtotal = Convert.ToDouble(dr["Subtotal"])
                    };
                    lista.Add(detalleVenta);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }
        public void InsertarDetalleVenta(entDetalleVenta detalle)
        {
            SqlCommand cmd = null;
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
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
    }
}
