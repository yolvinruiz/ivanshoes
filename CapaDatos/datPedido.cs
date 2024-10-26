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
    public class datPedido
    {
        #region singleton
        private static readonly datPedido _instancia = new datPedido();
        public static datPedido Instancia
        {
            get
            {
                return datPedido._instancia;
            }
        }
        #endregion singleton
        public bool InsertarPedido(entPedido pedido)
        {
            SqlCommand cmd = null;
            bool inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("InsertarPedido", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                cmd.Parameters.AddWithValue("@FechaEnvio", pedido.FechaEnvio);
                cmd.Parameters.AddWithValue("@FechaEntrega", pedido.FechaEntrega);
                cmd.Parameters.AddWithValue("@Estado", pedido.Estado);
                cmd.Parameters.AddWithValue("@Direccion", pedido.Direccion);
                cmd.Parameters.AddWithValue("@Ciudad", pedido.Ciudad);
                cmd.Parameters.AddWithValue("@NumeroSeguimiento", pedido.NumeroSeguimiento);
                cmd.Parameters.AddWithValue("@ID_venta", pedido.ID_venta);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    inserta = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Connection.Close();
            }
            return inserta;
        }
        public entPedido ObtenerInformacionPedidoCliente(int idPedido)
        {
            entPedido pedidoCliente = null;
            SqlCommand cmd = null;

            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    cmd = new SqlCommand("ObtenerInformacionPedidoCliente", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdPedido", idPedido);
                    cn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        pedidoCliente = new entPedido
                        {
                            NombreCliente = dr["NombreCliente"].ToString(),
                            DNI = Convert.ToInt32(dr["DNI"]),
                            Destino = dr["Destino"].ToString(),
                            FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }

            return pedidoCliente;
        }

        public List<entPedido> ListarPedidosHoy()
        {
            List<entPedido> listaPedidos = new List<entPedido>();
            SqlCommand cmd = null;

            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    cmd = new SqlCommand("SELECT *  FROM Pedidos WHERE FechaEnvio = CAST(GETDATE() AS DATE)", cn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cn.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        entPedido pedido = new entPedido
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"]),
                            FechaEntrega = Convert.ToDateTime(dr["FechaEntrega"]),
                            Estado = Convert.ToBoolean(dr["Estado"]),
                            Direccion = dr["Direccion"].ToString(),
                            Ciudad = dr["Ciudad"].ToString(),
                            NumeroSeguimiento = dr["NumeroSeguimiento"].ToString(),
                            ID_venta = Convert.ToInt32(dr["ID_venta"]),
                        };
                        listaPedidos.Add(pedido);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd?.Connection.Close();
            }

            return listaPedidos;
        }
        public bool ActualizarNumeroSeguimiento(int idPedido, string nuevoNumeroSeguimiento)
        {
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    cmd = new SqlCommand("UPDATE Pedidos SET NumeroSeguimiento = @NumeroSeguimiento WHERE Id = @Id", cn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", idPedido);
                    cmd.Parameters.AddWithValue("@NumeroSeguimiento", nuevoNumeroSeguimiento);

                    cn.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    resultado = filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el número de seguimiento", ex);
            }
            finally
            {
                cmd?.Connection.Close();
            }

            return resultado;
        }
    }
}
