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
    }
}
