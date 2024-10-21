using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entEmpleado
    {
        public int ID_Empleado { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int DNI { get; set; }
        public int Telefono { get; set; }
        public int id_Cargo { get; set; }
        public bool Estado { get; set; }
        public string NombreCargo { get; set; }
    }
}
