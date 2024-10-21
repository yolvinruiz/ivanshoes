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
    }
}
