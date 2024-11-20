
using AdministracionAgricola.Const;
using AdministracionAgricola.Interfaces;
using AdministracionAgricola.Servicios;
using Microsoft.AspNetCore.Identity;

namespace AdministracionAgricola.Configuration
{
    public class StartupService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public StartupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();

                var roles = new[] { AccountUserDefault.rol };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                string email = AccountUserDefault.email;
                string password = AccountUserDefault.password;

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser
                    {
                        UserName = email,
                        Email = email
                    };

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, AccountUserDefault.rol);

                    string idUser = user.Id;
                    await personService.createPerson(AccountPersonDefault.name, AccountPersonDefault.apellido, AccountPersonDefault.dni, idUser);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Servicio detenido.");
            return Task.CompletedTask;
        }
    }
}
