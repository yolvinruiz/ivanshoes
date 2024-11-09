using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace CapaDatos
{
    public class Conexion
    {
        private static readonly Conexion _instancia = new Conexion();
        public static Conexion Instancia
        {
            get { return _instancia; }
        }

        public SqlConnection Conectar()
        {
            try
            {
                string servidor = ConfigurationManager.AppSettings["Servidor"] ?? "DESKTOP-O490LA5";
                string baseDatos = ConfigurationManager.AppSettings["BaseDatos"] ?? "bdtiendap";

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = $"Data Source={servidor}; Initial Catalog={baseDatos}; Integrated Security=true";
                return cn;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al configurar la conexión: " + ex.Message);
            }
        }
    }
}
