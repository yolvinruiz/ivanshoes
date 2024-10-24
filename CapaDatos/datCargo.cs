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
    public class datCargo
    {

            private static readonly datCargo _instancia = new datCargo();
            public static datCargo Instancia
            {
                get { return datCargo._instancia; }
            }
            public List<entCargo> ListarCargo()
            {
                SqlCommand cmd = null;
                List<entCargo> lista = new List<entCargo>();
                try
                {
                    SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                    cmd = new SqlCommand("MostrarCargo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        entCargo mc = new entCargo();
                        mc.id_Cargo = Convert.ToInt32(dr["id_Cargo"]);
                        mc.Nombre = dr["Nombre"].ToString();
                        lista.Add(mc);
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

            public bool InsertarCargo(entCargo cargo)
            {
                SqlCommand cmd = null;
                bool inserta = false;
                try
                {
                    SqlConnection cn = Conexion.Instancia.Conectar();
                    cmd = new SqlCommand("InsertarCargo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", cargo.Nombre);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0) inserta = true;
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

            public bool ModificarCargo(entCargo cargo)
            {
                SqlCommand cmd = null;
                bool modifica = false;
                try
                {
                    SqlConnection cn = Conexion.Instancia.Conectar();
                    cmd = new SqlCommand("ModificarCargo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_Cargo", cargo.id_Cargo);
                    cmd.Parameters.AddWithValue("@Nombre", cargo.Nombre);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0) modifica = true;
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
            public bool EliminarCargo(int id_Cargo)
            {
                SqlCommand cmd = null;
                bool elimina = false;
                try
                {
                    SqlConnection cn = Conexion.Instancia.Conectar();
                    cmd = new SqlCommand("EliminarCargo", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_Cargo", id_Cargo);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0) elimina = true;
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

        }
    }
