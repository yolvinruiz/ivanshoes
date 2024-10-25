using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entPedido
    {
        public int Id { get; set; }
        public int idpedido { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime FechaEntrega { get; set; }
        public bool Estado { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string NumeroSeguimiento { get; set; }
    }
}
