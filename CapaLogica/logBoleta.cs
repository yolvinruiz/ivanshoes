using CapaEntidad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
namespace CapaLogica
{
    public class logBoleta
    {
        #region singleton
        private static readonly logBoleta _instancia = new logBoleta();
        public static logBoleta Instancia
        {
            get { return logBoleta._instancia; }
        }
        #endregion singleton

        public void InsertarBoleta(entBoleta boleta)
        {
            datBoleta.Instancia.InsertarBoleta(boleta);
        }

    }
 }

