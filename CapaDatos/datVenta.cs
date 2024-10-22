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
    public class datVenta
    {
        public static readonly datVenta _instancia = new datVenta();
        public static datVenta Instancia
        {
            get
            {
                return datVenta._instancia;
            }
        }
        public int InsertarVenta(entVenta venta)
        {
            SqlCommand cmd = null;
            int idVenta = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("InsertarVenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_Cliente", venta.id_cliente);
                cmd.Parameters.AddWithValue("@id_Empleado", venta.id_empleado);
                cmd.Parameters.AddWithValue("@Estado", venta.estado);

                // Parámetro de salida para obtener el ID de la venta
                SqlParameter outputIdParam = new SqlParameter();
                outputIdParam.ParameterName = "@ID_venta";
                outputIdParam.SqlDbType = SqlDbType.Int;
                outputIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputIdParam);

                cn.Open();
                cmd.ExecuteNonQuery();

                // Capturar el valor de salida del ID de la venta
                idVenta = Convert.ToInt32(outputIdParam.Value);
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
            return idVenta;
        }
    }
}
