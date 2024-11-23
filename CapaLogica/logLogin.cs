using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logLogin
    {
        #region singleton
        private static readonly logLogin _instancia = new logLogin();
        public static logLogin Instancia
        {
            get { return logLogin._instancia; }
        }
        #endregion singleton

        public string VerificarLogin(int idempleado, string contraseña)
        {
            return datLogin.Instancia.VerificarLogin(idempleado, contraseña);
        }
        public void CerrarSesion(int idEmpleado)
        {
            try
            {
                datLogin.Instancia.CerrarSesion(idEmpleado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar el cierre de sesión: " + ex.Message);
            }
        }

    }
}
