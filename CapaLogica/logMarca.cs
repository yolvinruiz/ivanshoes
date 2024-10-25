using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logMarca
    {
        #region sigleton
        private static readonly logMarca _instancia = new logMarca();
        public static logMarca Instancia
        {
            get
            {
                return logMarca._instancia;
            }
        }
        #endregion singleton
        public List<entMarca> Listarmarca()
        {
            return datMarca.Instancia.ListarMarca();
        }
        public void Insertarmarca(entMarca mc)
        {
            datMarca.Instancia.InsertarMarca(mc);
        }
        public void Editarmarca(entMarca mc)
        {
            datMarca.Instancia.ModificarMarca(mc);
        }
        public void Eliminarmarca(entMarca mc)
        {
            datMarca.Instancia.EliminarMarca(mc);
        }
    }
}
