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

        // Método para insertar un pedido
        public bool InsertarPedido(entPedido pedido)
        {
            return datPedido.Instancia.InsertarPedido(pedido);
        }
    }
}
