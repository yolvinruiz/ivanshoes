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
    public class datEmpleado
    {
        private static readonly datEmpleado _instancia = new datEmpleado();
        public static datEmpleado Instancia
        {
            get { return datEmpleado._instancia; }
        }
        public Boolean InsertarEmpleado(entEmpleado empleado)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("InsertarEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@apellidos", empleado.Apellidos);
                cmd.Parameters.AddWithValue("@dni", empleado.DNI);
                cmd.Parameters.AddWithValue("@telefono", empleado.Telefono);
                cmd.Parameters.AddWithValue("@correo", empleado.correo);
                cmd.Parameters.AddWithValue("@nombre_cargo", empleado.NombreCargo);
                cmd.Parameters.AddWithValue("@estado", empleado.Estado);

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
                cmd.Connection.Close();
            }
            return inserta;
        }

        public Boolean ModificarEmpleado(entEmpleado empleado)
        {
            SqlCommand cmd = null;
            Boolean modifica = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ModificarEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_empleado", empleado.ID_Empleado);
                cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@apellidos", empleado.Apellidos);
                cmd.Parameters.AddWithValue("@dni", empleado.DNI);
                cmd.Parameters.AddWithValue("@telefono", empleado.Telefono);
                cmd.Parameters.AddWithValue("@correo", empleado.correo);
                cmd.Parameters.AddWithValue("@nombre_cargo", empleado.NombreCargo);
                cmd.Parameters.AddWithValue("@estado", empleado.Estado);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    modifica = true;
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
            return modifica;
        }

        public Boolean EliminarEmpleado(int idEmpleado)
        {
            SqlCommand cmd = null;
            Boolean elimina = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("EliminarEmpleado", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_empleado", idEmpleado);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    elimina = true;
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
            return elimina;
        }

        public List<entEmpleado> ListarEmpleados()
        {
            SqlCommand cmd = null;
            List<entEmpleado> lista = new List<entEmpleado>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("MostrarEmpleados", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entEmpleado emp = new entEmpleado();
                    emp.ID_Empleado = Convert.ToInt32(dr["ID_Empleado"]);
                    emp.Nombre = dr["Nombre"].ToString();
                    emp.Apellidos = dr["Apellidos"].ToString();
                    emp.DNI = Convert.ToInt32(dr["DNI"]);
                    emp.Telefono = Convert.ToInt32(dr["Telefono"]);
                    emp.correo = dr["Correo"].ToString();
                    emp.id_Cargo = Convert.ToInt32(dr["id_cargo"]);
                    emp.Estado = Convert.ToBoolean(dr["Estado"]);
                    lista.Add(emp);
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
        public int ObtenerIdempledoPorDNI(int dni)
        {
            SqlCommand cmd = null;
            int idCliente = 0;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("SELECT ID_Empleado FROM Empleado WHERE Dni = @DNI", cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@DNI", dni);

                cn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    idCliente = Convert.ToInt32(result);
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
            return idCliente;
        }
        public Boolean ExisteEmpleadoPorDNI(int dni)
        {
            SqlCommand cmd = null;
            Boolean existe = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_VerificarEmpleadoPorDNI", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DNI", dni);

                cn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    existe = true;
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
            return existe;
        }
        public entEmpleado BuscarEmpleadoPorDNI(int dni)
        {
            SqlCommand cmd = null;
            entEmpleado empleado = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("BuscarEmpleadoPorDNI", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DNI", dni);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    empleado = new entEmpleado()
                    {
                        ID_Empleado = Convert.ToInt32(dr["ID_Empleado"]),
                        Nombre = dr["Nombre"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        DNI = Convert.ToInt32(dr["Dni"]),
                        Telefono = Convert.ToInt32(dr["Telefono"]),
                        correo = dr["Correo"].ToString(),
                        id_Cargo = Convert.ToInt32(dr["id_cargo"]),
                        NombreCargo = dr["NombreCargo"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return empleado;
        }
        public string obtenernombredecargo(int id)
        {
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("SELECT Nombre FROM Cargo WHERE id_Cargo = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }
        public int ObtenerIdPorNombre(string nombreCompleto)
        {
            int idEmpleado = 0;
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                try
                {
                    // Asegurarse de que el nombre esté limpio de espacios extras
                    nombreCompleto = string.Join(" ", nombreCompleto.Split(new[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries));

                    SqlCommand cmd = new SqlCommand("ObtenerIdPorNombreCompleto2", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);

                    cn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idEmpleado = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener ID del empleado: " + ex.Message);
                }
            }
            return idEmpleado;
        }
    }
}
