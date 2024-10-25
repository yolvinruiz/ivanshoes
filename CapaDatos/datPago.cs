using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datPago
    {
        public static readonly datPago _instancia = new datPago();
        public static datPago Instancia
        {
            get
            {
                return datPago._instancia;
            }
        }

        public bool RegistrarPagoYActualizarVenta(int idVenta, int idFormaPago)
        {
            SqlCommand cmd = null;
            bool exito = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("RegistrarPagoYActualizarVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_Venta", idVenta);
                cmd.Parameters.AddWithValue("@id_Forma_pago", idFormaPago);

                cn.Open();
                int resultado = cmd.ExecuteNonQuery();
                exito = (resultado > 0);
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
            return exito;
        }

    }
}
