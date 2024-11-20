using AdministracionAgricola.Interfaces;
using AdministracionAgricola.Models;
using Microsoft.AspNetCore.Identity;

namespace AdministracionAgricola.Servicios
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AdministracionCultivosContext _context;

        public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,AdministracionCultivosContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        } 

        public async Task<bool> CrearUsuarioAsync(string name, string apellido, int dni, string email, string password, string role)
        {
            // Verificar si el usuario ya existe
            if (await _userManager.FindByEmailAsync(email) != null)
                return false; // El usuario ya existe

            // Crear el usuario
            var usuario = new IdentityUser { UserName = email, Email = email };
            var person = new Persona { Nombre = name, Apellido = apellido, Dni = dni };
            var resultado = await _userManager.CreateAsync(usuario, password);
            person.PkAspNetUsers = usuario.Id;
            if (resultado.Succeeded)
            {
                var result = await _context.AspNetRoles.FindAsync(role);
                if (result!=null)
                {
                    // Asignar el usuario al rol
                    await _userManager.AddToRoleAsync(usuario,result.Name);
                    await _context.AddAsync(person);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public async Task<IdentityUser?> ObtenerUsuarioPorEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> AsignarRolAsync(string email, string role)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario == null) return false;

            // Crear el rol si no existe
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                    throw new Exception($"Error al crear el rol: {string.Join(", ", roleResult.Errors)}");
            }

            // Asignar el usuario al rol
            var resultado = await _userManager.AddToRoleAsync(usuario, role);
            return resultado.Succeeded;
        }
    }
}
