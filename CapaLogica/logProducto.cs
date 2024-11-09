using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaLogica
{
    public class logProducto
    {
        #region singleton
        private static readonly logProducto _instancia = new logProducto();
        public static logProducto Instancia
        {
            get { return logProducto._instancia; }
        }
        #endregion singleton

        public List<KeyValuePair<int, string>> ListarTiposProducto()
        {
            return datProducto.Instancia.ListarTiposProducto();
        }
        public List<KeyValuePair<int, string>> ListarMarcas()
        {
            return datProducto.Instancia.ListarMarcas();
        }

        public List<KeyValuePair<int, string>> ListarTallas()
        {
            return datProducto.Instancia.ListarTallas();
        }

        public List<KeyValuePair<int, string>> ListarColores()
        {
            return datProducto.Instancia.ListarColores();
        }

        public List<KeyValuePair<int, string>> ListarCategorias()
        {
            return datProducto.Instancia.ListarCategorias();
        }

        public void InsertarProducto(entProducto producto)
        {

            if (string.IsNullOrEmpty(producto.nombre))
                throw new Exception("El nombre es obligatorio");
            if (producto.precio <= 0)
                throw new Exception("El precio debe ser mayor a 0");
            if (producto.stock < 0)
                throw new Exception("El stock no puede ser negativo");

            datProducto.Instancia.InsertarProducto(producto);
        }

        public entProducto BuscarProductoPorId(int idProducto)
        {
            return datProducto.Instancia.BuscarProductoPorId(idProducto);
        }

        public void ModificarProducto(entProducto producto)
        {

            if (string.IsNullOrEmpty(producto.nombre))
                throw new Exception("El nombre es obligatorio");
            if (producto.precio <= 0)
                throw new Exception("El precio debe ser mayor a 0");
            if (producto.stock < 0)
                throw new Exception("El stock no puede ser negativo");

            datProducto.Instancia.ModificarProducto(producto);
        }
        public List<entProducto> ListarProducto()
        {
            return datProducto.Instancia.ListarProducto();
        }
        public void Insertarproducto(entProducto mc)
        {
            datProducto.Instancia.InsertarProducto(mc);
        }
       
        public List<entProducto> BuscarProductoConNombres(string termino)
        {
            return datProducto.Instancia.BuscarProductoConNombres(termino);
        }
        
    }
}
