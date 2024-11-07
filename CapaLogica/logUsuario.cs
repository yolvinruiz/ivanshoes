using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logUsuario
    {
        #region singleton
        private static readonly logUsuario _instancia = new logUsuario();
        public static logUsuario Instancia
        {
            get { return logUsuario._instancia; }
        }
        #endregion singleton

        public entCuentas ValidarUsuario(int dni, string contraseña)
        {
            return datCuentas.Instancia.ValidarUsuario(dni, contraseña);
        }
    }
}
