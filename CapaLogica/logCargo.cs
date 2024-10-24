using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logCargo
    {
        private static readonly logCargo _instancia = new logCargo();
        public static logCargo Instancia
        {
            get { return logCargo._instancia; }
        }

        public List<entCargo> ListarCargo()
        {
            return datCargo.Instancia.ListarCargo();
        }
        public void InsertarCargo(entCargo cargo)
        {
            datCargo.Instancia.InsertarCargo(cargo);
        }


        public void ModificarCargo(entCargo cargo)
        {
            datCargo.Instancia.ModificarCargo(cargo);
        }


        public void EliminarCargo(int id_Cargo)
        {
            datCargo.Instancia.EliminarCargo(id_Cargo);
        }
    }
}
