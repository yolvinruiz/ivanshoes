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
    }
}
