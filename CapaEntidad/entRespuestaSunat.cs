using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entRespuestaSunat
    {
        public string respuesta_sunat_codigo { get; set; }
        public string respuesta_sunat_descripcion { get; set; }
        public string ruta_xml { get; set; }
        public string ruta_cdr { get; set; }
        public string ruta_pdf { get; set; }
        public object codigo_hash { get; set; }
        public string xml_base_64 { get; set; }
        public string cdr_base_64 { get; set; }
    }
    public class entRootResponse
    {
        public entRespuestaSunat data { get; set; }
    }

}
