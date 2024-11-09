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
        public List<entBoleta> ListarComprobantes(DateTime fechaInicio, DateTime fechaFin)
        {
            List<entBoleta> lista = new List<entBoleta>();
            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ListarComprobantes", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                entBoleta comprobante = new entBoleta
                                {
                                    id_Boleta = Convert.ToInt32(dr["id_Boleta"]),
                                    id_Venta = Convert.ToInt32(dr["id_Venta"]),
                                    Serie = dr["Serie"].ToString(),
                                    Estado_sunat = dr["Estado_sunat"].ToString(),
                                    Pdf_filename = dr["Pdf_filename"].ToString(),
                                    Fecha = Convert.ToDateTime(dr["Fecha"]),
                                    Cliente = dr["Cliente"].ToString(),
                                    Total = Convert.ToDecimal(dr["Total"]),
                                    NroDocCliente = dr["NroDocCliente"].ToString()
                                };
                                lista.Add(comprobante);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar comprobantes: " + ex.Message);
            }
            return lista;
        }
        public List<entBoleta> ListarTodasBoletas()
        {
            List<entBoleta> lista = new List<entBoleta>();
            try
            {
                using (SqlConnection cn = Conexion.Instancia.Conectar())
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ListarTodasBoletas", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                entBoleta boleta = new entBoleta
                                {
                                    id_Boleta = Convert.ToInt32(dr["id_Boleta"]),
                                    id_Venta = Convert.ToInt32(dr["id_Venta"]),
                                    Serie = dr["Serie"].ToString(),
                                    DigestValueon = dr["DigestValueon"].ToString(),
                                    Estado_sunat = dr["Estado_sunat"].ToString(),
                                    Mensaje_sunat = dr["Mensaje_sunat"].ToString(),
                                    Xml_filename = dr["Xml_filename"].ToString(),
                                    Pdf_filename = dr["Pdf_filename"].ToString(),
                                    Cdr_filename = dr["Cdr_filename"].ToString(),
                                    // Datos adicionales de la venta y cliente
                                    Fecha = Convert.ToDateTime(dr["Fecha"]),
                                    Cliente = dr["Cliente"].ToString(),
                                    Total = Convert.ToDecimal(dr["Total"])
                                };
                                lista.Add(boleta);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar boletas: " + ex.Message);
            }
            return lista;
        }
    }
}
