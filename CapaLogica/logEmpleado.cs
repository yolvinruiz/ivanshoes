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
        public int ObtenerIdPorNombre(string nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new Exception("El nombre del empleado es requerido");

            // Dividir y validar el formato del nombre
            string[] partes = nombreCompleto.Split(new char[] { ' ' }, 2);
            if (partes.Length < 2)
                throw new Exception("El formato del nombre no es válido. Debe incluir nombre y apellidos");

            // Obtener el ID
            int idEmpleado = datEmpleado.Instancia.ObtenerIdPorNombre(nombreCompleto);

            if (idEmpleado == 0)
                throw new Exception("No se encontró el empleado con el nombre especificado");

            return idEmpleado;
        }
    }
}
