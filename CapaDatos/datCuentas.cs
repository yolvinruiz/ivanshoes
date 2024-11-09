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
        public void CrearCuenta(entCuentas cuenta)
        {
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("CrearCuenta", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDempleado", cuenta.IDempleado);
                    cmd.Parameters.AddWithValue("@Contraseña", cuenta.Contraseña);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("ya tiene una cuenta"))
                        throw new Exception("El empleado ya tiene una cuenta asociada.");
                    else
                        throw new Exception("Error al crear la cuenta: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al crear la cuenta: " + ex.Message);
                }
            }
        }

        public bool VerificarCuentaExistente(int idEmpleado)
        {
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Cuentas WHERE IDempleado = @IDempleado", cn);
                    cmd.Parameters.AddWithValue("@IDempleado", idEmpleado);

                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al verificar cuenta existente: " + ex.Message);
                }
            }
        }
        public void ModificarContraseña(int idEmpleado, string contraseñaActual, string contraseñaNueva)
        {
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("ModificarContraseña", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDempleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@ContraseñaActual", contraseñaActual);
                    cmd.Parameters.AddWithValue("@ContraseñaNueva", contraseñaNueva);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("contraseña actual es incorrecta"))
                        throw new Exception("La contraseña actual es incorrecta.");
                    else
                        throw new Exception("Error al modificar la contraseña: " + ex.Message);
                }
            }
        }

    }
}
