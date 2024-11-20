using AdministracionAgricola.Configuration;
using AdministracionAgricola.Data; // Importa los espacios de nombres para los DbContexts
using AdministracionAgricola.Interfaces;
using AdministracionAgricola.Models; // Importa los modelos de datos de la aplicación
using AdministracionAgricola.Servicios;
using Microsoft.AspNetCore.Identity; // Importa las funcionalidades relacionadas con Identity
using Microsoft.EntityFrameworkCore; // Importa Entity Framework Core para trabajar con bases de datos

namespace RegistrarUsuario // Define el espacio de nombres para el proyecto
{
    public class Program  // Clase principal que configura y ejecuta la aplicación
    {
        public static void Main(string[] args)  // Método principal que ejecuta la aplicación
        {
            
            // Crea el builder para la configuración de la aplicación 
            var builder = WebApplication.CreateBuilder(args);

            // Recupera la cadena de conexión desde el archivo de configuración (appsettings.json)
            var connectionStrin = builder.Configuration.GetConnectionString("ApplicationDbContext");

            // Configura el DbContext para la base de datos de la aplicación utilizando la cadena de conexión
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Agrega un filtro para mostrar detalles de la base de datos en desarrollo

            // Configura otro DbContext (AdministracionCultivosContext) para una base de datos diferente
            builder.Services.AddDbContext<AdministracionCultivosContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Filtro similar al anterior para esta base de datos

            // Configura Identity para manejar autenticación y roles de usuario
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = false) // Establece que no se requiera una cuenta confirmada para iniciar sesión
                .AddRoles<IdentityRole>() // Agrega roles para usuarios
                .AddEntityFrameworkStores<ApplicationDbContext>(); // Establece el contexto de base de datos para Identity

            // Agrega soporte para controladores con vistas (MVC)
            builder.Services.AddControllersWithViews();
            // Registrar el servicio hospedado StartupService
            // Registro de IPersonService u otros servicios necesarios
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPersonService, PersonService>();

            builder.Services.AddScoped<IRolService, RolService>(); // Registro del servicio para la gestión de roles

            // Registrar el servicio de fondo
            builder.Services.AddHostedService<StartupService>();

            //Listar la tabla Usuarios,persona y roles
            builder.Services.AddScoped<IListarUsuarioService, ListarUsuarioService>();


            // Agrega soporte para Razor Pages (páginas dinámicas)
            builder.Services.AddRazorPages();

            var app = builder.Build(); // Construye la aplicación

            // Configura la tubería de solicitud HTTP (el pipeline)
            if (app.Environment.IsDevelopment())  // Si está en el entorno de desarrollo
            {
                app.UseMigrationsEndPoint(); // Habilita el punto de entrada para las migraciones en desarrollo
            }
            else  // Si está en el entorno de producción
            {
                app.UseExceptionHandler("/Home/Error"); // Muestra una página de error genérica
            }

            // Redirige todas las solicitudes HTTP a HTTPS
            app.UseHttpsRedirection();

            app.UseStaticFiles(); // Permite servir archivos estáticos (CSS, JS, imágenes)

            app.UseRouting(); // Habilita el enrutamiento de solicitudes

            app.UseAuthentication(); // Habilita la autenticación (verifica que los usuarios estén logueados)

            app.UseAuthorization(); // Habilita la autorización (verifica que el usuario tenga permisos para acceder)

            //// Configura los puntos finales de la aplicación
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages(); // Mapea las páginas Razor
            //});

            // Configura la ruta predeterminada para los controladores
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=LoginPage}/{id?}"); // Ruta predeterminada con controlador 'Home' y acción 'LoginPage'

            app.MapRazorPages(); // Mapea las páginas Razor

            // Crear roles predeterminados al iniciar la aplicación
            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    var roleService = scope.ServiceProvider.GetRequiredService<IRolService>();

            //    var roles = new[] { "Administrador", "Empleado" };

            //    foreach (var role in roles)
            //    {
            //        var roleExists = await roleManager.RoleExistsAsync(role);
            //        if (!roleExists)
            //        {
            //            await roleService.CreateRolAsync(role); // Usar el servicio para crear el rol
            //        }
            //    }
            //}

            app.Run(); // Inicia la aplicación y espera solicitudes

        }
    }
}
