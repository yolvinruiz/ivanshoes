using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logPedido
    {
        #region singleton
        private static readonly logPedido _instancia = new logPedido();
        public static logPedido Instancia
        {
            get
            {
                return logPedido._instancia;
            }
        }
        #endregion singleton
        public bool InsertarPedido(entPedido pedido)
        {
            return datPedido.Instancia.InsertarPedido(pedido);
        }
        public entPedido ObtenerInformacionPedidoCliente(int idPedido)
        {
            return datPedido.Instancia.ObtenerInformacionPedidoCliente(idPedido);
        }
        public List<entPedido> ListarPedidosHoy()
        {
            return datPedido.Instancia.ListarPedidosHoy();
        }
        public bool ActualizarNumeroSeguimiento(int idPedido, string nuevoNumeroSeguimiento)
        {
            return datPedido.Instancia.ActualizarNumeroSeguimiento(idPedido, nuevoNumeroSeguimiento);
        }
    }
}
