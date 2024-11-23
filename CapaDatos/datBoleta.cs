using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datBoleta
    {
        #region singleton
        private static readonly datBoleta _instancia = new datBoleta();
        public static datBoleta Instancia
        {
            get { return datBoleta._instancia; }
        }
        #endregion singleton

        public Boolean InsertarBoleta(entBoleta boleta)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("InsertarBoleta", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_Venta", boleta.id_Venta);
                cmd.Parameters.AddWithValue("@Serie", boleta.Serie);
                cmd.Parameters.AddWithValue("@DigestValueon", boleta.DigestValueon);
                cmd.Parameters.AddWithValue("@Estado_sunat", boleta.Estado_sunat);
                cmd.Parameters.AddWithValue("@Mensaje_sunat", boleta.Mensaje_sunat);
                cmd.Parameters.AddWithValue("@Xml_filename", boleta.Xml_filename);
                cmd.Parameters.AddWithValue("@Pdf_filename", boleta.Pdf_filename);
                cmd.Parameters.AddWithValue("@Cdr_filename", boleta.Cdr_filename);

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
        public List<entBoleta> BuscarBoletasPorDni(int dni)
        {
            List<entBoleta> boletas = new List<entBoleta>();

            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("BuscarBoletasPorDNI", cn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@DNI", dni);

                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            boletas.Add(new entBoleta
                            {
                                id_Boleta = reader.GetInt32(reader.GetOrdinal("id_Boleta")),
                                id_Venta = reader.GetInt32(reader.GetOrdinal("id_Venta")),
                                Serie = reader.GetString(reader.GetOrdinal("Serie")),
                                DigestValueon = reader.GetString(reader.GetOrdinal("DigestValueon")),
                                Estado_sunat = reader.GetString(reader.GetOrdinal("Estado_sunat")),
                                Mensaje_sunat = reader.GetString(reader.GetOrdinal("Mensaje_sunat")),
                                Xml_filename = reader.GetString(reader.GetOrdinal("Xml_filename")),
                                Pdf_filename = reader.GetString(reader.GetOrdinal("Pdf_filename")),
                                Cdr_filename = reader.GetString(reader.GetOrdinal("Cdr_filename")),
                                Cliente = reader.GetString(reader.GetOrdinal("Cliente")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar las boletas: " + ex.Message);
            }

            return boletas;
        }
        public List<entBoleta> ListarTodasLasBoletas()
        {
            List<entBoleta> boletas = new List<entBoleta>();

            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("ListarTodasBoletas", cn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            boletas.Add(new entBoleta
                            {
                                id_Boleta = reader.GetInt32(reader.GetOrdinal("id_Boleta")),
                                id_Venta = reader.GetInt32(reader.GetOrdinal("id_Venta")),
                                Serie = reader.GetString(reader.GetOrdinal("Serie")),
                                DigestValueon = reader.GetString(reader.GetOrdinal("DigestValueon")),
                                Estado_sunat = reader.GetString(reader.GetOrdinal("Estado_sunat")),
                                Mensaje_sunat = reader.GetString(reader.GetOrdinal("Mensaje_sunat")),
                                Xml_filename = reader.GetString(reader.GetOrdinal("Xml_filename")),
                                Pdf_filename = reader.GetString(reader.GetOrdinal("Pdf_filename")),
                                Cdr_filename = reader.GetString(reader.GetOrdinal("Cdr_filename")),
                                Cliente = reader.GetString(reader.GetOrdinal("Cliente")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha"))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar todas las boletas: " + ex.Message);
            }

            return boletas;
        }
    }
}
