using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entCuentas
    {
        public int IDcuenta { get; set; }
        public int IDempleado { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Cargo { get; set; }
        public int DNI { get; set; }
        public string Contraseña { get; set; }
    }
}
