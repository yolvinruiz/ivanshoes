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
        public List<entBoleta> ListarComprobantes(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return datBoleta.Instancia.ListarComprobantes(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa lógica: " + ex.Message);
            }
        }
        public string ObtenerTipoComprobante(string serie)
        {
            if (string.IsNullOrEmpty(serie))
                return "Desconocido";

            return serie.StartsWith("BB01") ? "Boleta" :
                   serie.StartsWith("FF03") ? "Factura" :
                   "Desconocido";
        }
        public List<entBoleta> ListarTodasBoletas()
        {
            try
            {
                return datBoleta.Instancia.ListarTodasBoletas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la capa lógica: " + ex.Message);
            }
        }

    }
 }

