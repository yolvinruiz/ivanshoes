using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
namespace CapaLogica
{
    public class logCuentas
    {
        #region singleton
        private static readonly logCuentas _instancia = new logCuentas();
        public static logCuentas Instancia
        {
            get { return logCuentas._instancia; }
        }
        #endregion singleton
        public void CrearCuenta(entCuentas cuenta)
        {
            try
            {
                // Validaciones
                if (cuenta.IDempleado <= 0)
                    throw new Exception("ID de empleado no válido");

                if (string.IsNullOrWhiteSpace(cuenta.Contraseña))
                    throw new Exception("La contraseña es requerida");

                if (cuenta.Contraseña.Length < 6)
                    throw new Exception("La contraseña debe tener al menos 6 caracteres");

                // Verificar si el empleado ya tiene cuenta
                if (datCuentas.Instancia.VerificarCuentaExistente(cuenta.IDempleado))
                    throw new Exception("El empleado ya tiene una cuenta asociada");

                // Crear la cuenta
                datCuentas.Instancia.CrearCuenta(cuenta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ModificarContraseña(int idEmpleado, string contraseñaActual, string contraseñaNueva)
        {
            if (string.IsNullOrWhiteSpace(contraseñaActual))
                throw new Exception("La contraseña actual es requerida");

            if (string.IsNullOrWhiteSpace(contraseñaNueva))
                throw new Exception("La nueva contraseña es requerida");

            if (contraseñaNueva.Length < 6)
                throw new Exception("La nueva contraseña debe tener al menos 6 caracteres");

            if (contraseñaActual == contraseñaNueva)
                throw new Exception("La nueva contraseña debe ser diferente a la actual");

            datCuentas.Instancia.ModificarContraseña(idEmpleado, contraseñaActual, contraseñaNueva);
        }
    }
}
