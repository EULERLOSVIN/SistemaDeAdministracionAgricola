using AdministracionAgricola.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AdministracionAgricola.Servicios
{
    public class RolService : IRolService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        // Inyectamos el RoleManager en el constructor
        public RolService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRolAsync(string nameRol)
        {
            // Normalizar el nombre del rol, eliminando espacios y convirtiéndolo a mayúsculas
            var normalizedRoleName = nameRol.Trim();

            // Verificar si el rol ya existe
            var roleExists = await _roleManager.RoleExistsAsync(normalizedRoleName);
            if (roleExists)
            {
                return false;  // El rol ya existe, no lo creamos
            }

            // Crear el nuevo rol
            var newRole = new IdentityRole(normalizedRoleName);
            var result = await _roleManager.CreateAsync(newRole);

            // Verificar si la creación fue exitosa
            if (result.Succeeded)
            {
                return true;  // Rol creado exitosamente
            }
            else
            {
                // Si hubo errores, loguearlos (esto te ayudará a depurar)
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error al crear el rol: {error.Description}");
                    // Si tienes un logger configurado, puedes usarlo aquí
                    // _logger.LogError($"Error al crear el rol: {error.Description}");
                }
                return false;  // Retorna falso si hubo un error en la creación
            }
        }
    }
}
