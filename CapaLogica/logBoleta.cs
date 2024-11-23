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
        public List<entBoleta> BuscarBoletasPorDni(int dni)
        {
            try
            {
                return datBoleta.Instancia.BuscarBoletasPorDni(dni);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar la búsqueda de boletas: " + ex.Message);
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
        public List<entBoleta> ListarTodasLasBoletas()
        {
            try
            {
                return datBoleta.Instancia.ListarTodasLasBoletas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al procesar el listado de boletas: " + ex.Message);
            }
        }

    }
 }

