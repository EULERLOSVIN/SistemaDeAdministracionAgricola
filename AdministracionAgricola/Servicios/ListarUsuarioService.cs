using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AdministracionAgricola.Data;
using AdministracionAgricola.Interfaces;
using AdministracionAgricola.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdministracionAgricola.Servicios
{
    public class ListarUsuarioService : IListarUsuarioService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AdministracionCultivosContext _dbAdminAgricolaContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        // Constructor que inyecta ApplicationDbContext y AdministracionCultivosContext
        public ListarUsuarioService(ApplicationDbContext dbContext, AdministracionCultivosContext dbAdminAgricolaContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _dbAdminAgricolaContext = dbAdminAgricolaContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

       

        // Método para obtener los usuarios con detalles combinados
        public async Task<List<UsuarioDetalle>> ListarTodoUsuarios()
        {

            List<Persona> x = await _dbAdminAgricolaContext.Personas.Include(x => x.PkAspNetUsersNavigation).ToListAsync();
            List<UsuarioDetalle> _usuarioDetalle = new List<UsuarioDetalle>();

            foreach (var item in x)
            {
                // Crear un nuevo objeto de UsuarioDetalle por cada elemento
                var detalle = new UsuarioDetalle
                {
                    nombre = item.Nombre,
                    apellido = item.Apellido,
                    dni = item.Dni,
                    email = item.PkAspNetUsersNavigation.Email,
                    rol = await ObtenerRolesConTipoAsync(item.PkAspNetUsersNavigation.Id)
                };

                // Agregar el objeto detalle a la lista
                _usuarioDetalle.Add(detalle);
            }
            return _usuarioDetalle;
        }

        public async Task<string> ObtenerRolesConTipoAsync(string userId)
        {
            // Buscar al usuario por ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // Si no se encuentra al usuario, devolver una lista vacía
                return null;
            }

            // Obtener los roles asociados a este usuario
            var roles = await _userManager.GetRolesAsync(user);
            return string.Join(", ", roles);
        }

        public async Task<UsuarioDetalle> ObtenerUsuarioPorIdAsync(string id)
        {
            // Buscar la persona que tenga un usuario asociado con el ID proporcionado
            var persona = await _dbAdminAgricolaContext.Personas.Include(p => p.PkAspNetUsersNavigation).FirstOrDefaultAsync(p => p.PkAspNetUsersNavigation.Id == id);

            if (persona == null)
            {
                return null; // Retorna null si no se encuentra el usuario
            }

            // Obtener el usuario de Identity con UserManager
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null; // Retorna null si el usuario no se encuentra en UserManager
            }

            // Obtener los roles asociados al usuario
            var roles = await ObtenerRolesConTipoAsync(id);

            // Crear y devolver un objeto UsuarioDetalle con la información obtenida
            var usuarioDetalle = new UsuarioDetalle
            {
                nombre = persona.Nombre,
                apellido = persona.Apellido,
                dni = persona.Dni,
                email = user.Email,
                rol = roles
            };

            return usuarioDetalle;
        }

    }
}
