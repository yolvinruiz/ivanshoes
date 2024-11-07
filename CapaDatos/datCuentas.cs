using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class datCuentas
    {
        #region singleton
        private static readonly datCuentas _instancia = new datCuentas();
        public static datCuentas Instancia
        {
            get { return datCuentas._instancia; }
        }
        #endregion singleton

        public entCuentas ValidarUsuario(int dni, string contraseña)
        {
            SqlCommand cmd = null;
            entCuentas usuario = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ValidarLogin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DNI", dni);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    usuario = new entCuentas()
                    {
                        IDcuenta = Convert.ToInt32(dr["IDcuenta"]),
                        IDempleado = Convert.ToInt32(dr["IDempleado"]),
                        Nombre = dr["Nombre"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        Cargo = dr["Cargo"].ToString()
                    };
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
            return usuario;
        }
    }
}
