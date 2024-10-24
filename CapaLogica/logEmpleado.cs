using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logEmpleado
    {
        private static readonly logEmpleado _instancia = new logEmpleado();
        public static logEmpleado Instancia
        {
            get { return logEmpleado._instancia; }
        }

        public void InsertarEmpleado(entEmpleado emp)
        {
            datEmpleado.Instancia.InsertarEmpleado(emp);
        }

        public void ModificarEmpleado(entEmpleado emp)
        {
            datEmpleado.Instancia.ModificarEmpleado(emp);
        }

        public void EliminarEmpleado(int idEmpleado)
        {
            datEmpleado.Instancia.EliminarEmpleado(idEmpleado);
        }

        public List<entEmpleado> ListarEmpleados()
        {
            return datEmpleado.Instancia.ListarEmpleados();
        }
        public int BuscarIdempleadoPorDNI(int dni)
        {
            return datEmpleado.Instancia.ObtenerIdempledoPorDNI(dni);
        }
        public Boolean VerificarEmpleadoPorDNI(int dni)
        {
            return datEmpleado.Instancia.ExisteEmpleadoPorDNI(dni);
        }
        public string obtenernombredecargo(int x)
        {
            return datEmpleado.Instancia.obtenernombredecargo(x);
        }
        public entEmpleado bucarempleadopordni(int dni)
        {
            return datEmpleado.Instancia.BuscarEmpleadoPorDNI(dni);
        }
    }
}
