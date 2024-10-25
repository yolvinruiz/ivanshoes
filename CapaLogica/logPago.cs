using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logPago
    {
        public static readonly logPago _instancia = new logPago();
        public static logPago Instancia
        {
            get
            {
                return logPago._instancia;
            }
        }

        public bool RegistrarPagoYActualizarVenta(int idVenta, int idFormaPago)
        {
            return datPago.Instancia.RegistrarPagoYActualizarVenta(idVenta, idFormaPago);
        }
    }
}
