using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCliente
    {
        #region sigleton
        private static readonly logCliente _instancia = new logCliente();
        public static logCliente Instancia
        {
            get
            {
                return logCliente._instancia;
            }
        }
        #endregion singleton
        public List<entCliente> Listarcliente()
        {
            return datCliente.Instancia.ListarCliente();
        }
        public void InsertarCliente(entCliente mc)
        {
            datCliente.Instancia.InsertarCliente(mc);
        }
        public void EditarCliente(entCliente mc)
        {
            datCliente.Instancia.ModificarCliente(mc);
        }
        public void EliminarCliente(int mc)
        {
            datCliente.Instancia.EliminarCliente(mc);

        }
        public Boolean VerificarClientePorDNI(int dni)
        {
            return datCliente.Instancia.ExisteClientePorDNI(dni);
        }
        public int BuscarIdClientePorDNI(int dni)
        {
            return datCliente.Instancia.ObtenerIdClientePorDNI(dni);
        }
    }
}
