using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdministracionAgricola.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AdministracionAgricola.Models;
using System.Data;

namespace AdministracionAgricola.Controllers
{
    
    public class AutenticacionController : Controller
    {
        private readonly IUserService _userservice;
        private readonly IRolService _rolService;  // Inyecta IRolService para manejar roles

        private readonly RoleManager<IdentityRole> _roleManager;  // Inyecta RoleManager para acceder a los roles

        private readonly AdministracionCultivosContext _dbAdminAgricolaContext;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IListarUsuarioService _listarUsuarioService;

        public AutenticacionController(IUserService userservice, IRolService rolService, RoleManager<IdentityRole> roleManager, IListarUsuarioService listarUsuarioService, UserManager<IdentityUser> userManager, AdministracionCultivosContext dbAdminAgricolaContext)
        {
            _userservice = userservice;
            _rolService = rolService;
            _roleManager = roleManager;
            _listarUsuarioService = listarUsuarioService;  // Inyecta IListarUsuarioService
            _userManager = userManager;
            _dbAdminAgricolaContext = dbAdminAgricolaContext;
        }

        // Acción GET para mostrar el formulario de creación de rol
        [HttpGet]
        public IActionResult Rol()
        {
            return View();  // Muestra la vista del formulario de creación de rol
        }

        // Acción POST para manejar la creación de rol
        [HttpPost]
        public async Task<IActionResult> Rol(string name)
        {
            // Validamos que el nombre del rol no esté vacío
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError("", "El nombre del rol no puede estar vacío.");
                return View();
            }

            // Llamamos al servicio para crear el rol
            var rolCreado = await _rolService.CreateRolAsync(name);
            if (rolCreado)
            {
                // Si el rol se crea con éxito, redirigimos a otra acción (por ejemplo, a la gestión de usuarios)
                ViewBag.Message = "Rol creado exitosamente.";
                return RedirectToAction("GestionUser");  // Redirige a la acción 'GestionUser' o donde quieras
            }
            else
            {
                // Si el rol ya existe o hubo un error, mostramos un mensaje de error
                ModelState.AddModelError("", "El rol ya existe o hubo un error al crearlo.");
                return View();
            }
        }

        // Acción para gestionar los usuarios

        // Acción para gestionar los usuarios
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GestionUser()
        {
            // Obtener los roles existentes de la base de datos
            var roles = await _roleManager.Roles.ToListAsync();

            // Obtener la lista de usuarios con detalles
            List<UsuarioDetalle> detalle = await _listarUsuarioService.ListarTodoUsuarios();


            // Pasar los roles a la vista
            ViewBag.Roles = roles;

            return View(detalle);
        }

        // Acción POST para crear un usuario
        [HttpPost]
        public async Task<IActionResult> GestionUser(string name, string apellido, int dni, string email, string password, string role)
        {
            if (ModelState.IsValid)
            {
                // Crear el usuario utilizando el servicio de usuario
                var creado = await _userservice.CrearUsuarioAsync(name, apellido, dni, email, password, role);
                if (creado)
                {
                    ViewBag.Mensaje = "Usuario creado exitosamente.";
                    return RedirectToAction("GestionUser");
                }
                ModelState.AddModelError("", "El usuario ya existe.");
            }
            return View();
        }




        [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult RegistroPlantacion()
        {
            return View();
        }
         
        [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult RegistroProduccion()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult ControlInsumos()
        {
            return View();
        }

        [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult ControlPerdidas()
        {
            return View();
        }
        [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult GestionRiego()
        {
            return View();
        }
        [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult MonitoreoClima()
        {
            return View();
        }
        [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult GeneracionReportes()
        {
            return View();
        }

         [Authorize(Roles = "Administrador,Capataz")]
        public IActionResult Notificaciones()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditarUser(UsuarioDetalle item)
        {
            return View(item);
        }


        [HttpPost]
        public async Task<IActionResult> EditarUser(string name, string lastname, int _dni, string _email, string _rol)
        {
            // Verificar si los datos son válidos
            if (ModelState.IsValid)
            {
                // Buscar la persona en la base de datos
                var personAeditar = _dbAdminAgricolaContext.Personas.Include(x => x.PkAspNetUsersNavigation).FirstOrDefault(u => u.Dni == _dni);//FirstOrDefault(u => u.Dni == _dni)

                if (personAeditar != null)
                {
                    // Buscar el usuario de Identity relacionado con la persona
                    var userAeditar = await _userManager.FindByIdAsync(personAeditar.PkAspNetUsers);

                    if (userAeditar != null)
                    {
                        // Obtener los roles actuales del usuario
                        var rolAeditar = await _userManager.GetRolesAsync(userAeditar);


                        personAeditar.Nombre = name;
                        personAeditar.Apellido = lastname;
                        personAeditar.Dni = _dni;
                        await _userManager.SetEmailAsync(userAeditar, _email);

                        // Verificar si el rol ya está asignado, si no, asignarlo
                        if (!rolAeditar.Contains(_rol))
                        {
                            // Asignar el nuevo rol al usuario
                            await _userManager.RemoveFromRolesAsync(userAeditar,rolAeditar);
                            await _userManager.AddToRoleAsync(userAeditar, _rol);
                        }

                        // Guardar los cambios en la base de datos
                        await _dbAdminAgricolaContext.SaveChangesAsync();
                        


                        // Redirigir a la vista de gestión de usuarios
                        return RedirectToAction("GestionUser");

                        
                    }
                    else
                    {
                        // Si el usuario no se encuentra, mostrar un error
                        ModelState.AddModelError("", "Usuario no encontrado.");
                    }
                }
                else
                {
                    // Si la persona no se encuentra, mostrar un error
                    ModelState.AddModelError("", "Persona no encontrada.");
                }
            }

            // Si el modelo no es válido, volvemos a la vista de edición
            return View();
        }

        [HttpGet]
        public IActionResult EliminarUser(UsuarioDetalle item)
        {
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarUser(string name, string lastname, int _dni, string _email, string _rol)
        {
            // Verificar si el modelo es válido
            if (ModelState.IsValid)
            {
                // Buscar la persona en la base de datos junto con la navegación al usuario de Identity
                var personAeliminar = _dbAdminAgricolaContext.Personas.Include(x => x.PkAspNetUsersNavigation).FirstOrDefault(u => u.Dni == _dni);// Incluye la navegación de AspNetUser


                if (personAeliminar != null)
                {
                    // Eliminar primero la persona
                    _dbAdminAgricolaContext.Personas.Remove(personAeliminar);
                    await _dbAdminAgricolaContext.SaveChangesAsync();

                    // Luego, buscar y eliminar el usuario de Identity relacionado con la persona
                    var userAeliminar = await _userManager.FindByIdAsync(personAeliminar.PkAspNetUsers);

                    if (userAeliminar != null)
                    {
                        // Obtener los roles actuales asignados al usuario
                        var rolesAsignados = await _userManager.GetRolesAsync(userAeliminar);

                        // Eliminar los roles asignados al usuario
                        foreach (var rol in rolesAsignados)
                        {
                            await _userManager.RemoveFromRoleAsync(userAeliminar, rol);
                        }

                        // Eliminar el usuario de Identity de la base de datos
                        var resultadoEliminacionUsuario = await _userManager.DeleteAsync(userAeliminar);

                        if (resultadoEliminacionUsuario.Succeeded)
                        {
                            // Redirigir a la vista de gestión de usuarios
                            return RedirectToAction("GestionUser");
                        }
                        else
                        {
                            // Si hubo un error al eliminar el usuario, mostrar un error
                            ModelState.AddModelError("", "Error al eliminar el usuario de la base de datos.");
                        }
                    }
                    else
                    {
                        // Si el usuario no se encuentra, mostrar un error
                        ModelState.AddModelError("", "Usuario no encontrado.");
                    }
                }
                else
                {
                    // Si la persona no se encuentra, mostrar un error
                    ModelState.AddModelError("", "Persona no encontrada.");
                }
            }

            // Si el modelo no es válido, volvemos a la vista de eliminación
            return View();
        }






    }
}
