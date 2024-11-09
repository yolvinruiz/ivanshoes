using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entBoleta
    {
        public int id_Boleta { get; set; }
        public int id_Venta { get; set; }
        public string Serie { get; set; }
        public string DigestValueon { get; set; }
        public string Estado_sunat { get; set; }
        public string Mensaje_sunat { get; set; }
        public string Xml_filename { get; set; }
        public string Pdf_filename { get; set; }
        public string Cdr_filename { get; set; }

        public string TipoComprobante { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public string NroDocCliente { get; set; }
    }
}
