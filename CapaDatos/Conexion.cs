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
                string servidor = ConfigurationManager.AppSettings["Servidor"] ?? "localhost";
                string baseDatos = ConfigurationManager.AppSettings["BaseDatos"] ?? "bdtiendap";
                string usuario = ConfigurationManager.AppSettings["Usuario"] ?? "";
                string contraseña = ConfigurationManager.AppSettings["Contraseña"] ?? "";

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = $"Server=tcp:{servidor},1433; Database={baseDatos}; User ID={usuario}; Password={contraseña}; Encrypt=True; TrustServerCertificate=False; Connection Timeout=30;";
                return cn;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al configurar la conexión: " + ex.Message);
            }
        }
    }
}
