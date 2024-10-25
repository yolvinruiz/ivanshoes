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

        public bool VerificarLogin(string dni, string contraseña)
        {
            SqlCommand cmd = null;
            bool resultado = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("VerificarCuenta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dni", dni);
                cmd.Parameters.AddWithValue("@contraseña", contraseña);
                cn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            return resultado;
        }
    }
}
