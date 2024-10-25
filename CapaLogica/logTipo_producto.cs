using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logTipo_producto
    {
        #region sigleton
        private static readonly logTipo_producto _instancia = new logTipo_producto();
        public static logTipo_producto Instancia
        {
            get
            {
                return logTipo_producto._instancia;
            }
        }
        #endregion singleton
        public List<entTipo_producto> ListartipoProducto()
        {
            return datTipo_producto.Instancia.ListartipoProducto();
        }
        public void Insertartipoproducto(entTipo_producto tip)
        {
            datTipo_producto.Instancia.Insertartipoproducto(tip);
        }
        public void EditarTipoproducto(entTipo_producto Cli)
        {
            datTipo_producto.Instancia.Editartipoproducto(Cli);
        }
        public void Eliminartipoproducto(entTipo_producto Cli)
        {
            datTipo_producto.Instancia.Eliminartipoproducto(Cli);
        }

    }
}
