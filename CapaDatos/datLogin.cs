using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datLogin
    {
        #region singleton
        public static readonly datLogin _instancia = new datLogin();
        public static datLogin Instancia
        {
            get { return datLogin._instancia; }
        }
        #endregion singleton

        public string VerificarLogin(int idEmpleado, string contraseña)
        {
            SqlCommand cmd = null;
            string mensaje = string.Empty;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();

                cmd = new SqlCommand("LoginCuenta", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@IDempleado", idEmpleado);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);

                // Parámetro de salida
                SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(mensajeParam);

                cn.Open();
                cmd.ExecuteNonQuery();

                // Leer el mensaje de salida
                mensaje = mensajeParam.Value.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar el login: " + ex.Message);
            }
            finally
            {
                if (cmd?.Connection?.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return mensaje;
        }
        public void CerrarSesion(int idEmpleado)
        {
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                SqlCommand cmd = new SqlCommand("CerrarSesion", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetro de entrada
                cmd.Parameters.AddWithValue("@IDempleado", idEmpleado);

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cerrar la sesión: " + ex.Message);
            }
        }

    }
}
