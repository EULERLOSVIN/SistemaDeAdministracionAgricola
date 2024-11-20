using Microsoft.AspNetCore.Identity;

namespace AdministracionAgricola.Interfaces
{
    public interface IUserService
    {
        Task<bool> CrearUsuarioAsync(string name, string apellido, int dni, string email, string password, string role);
        Task<IdentityUser?> ObtenerUsuarioPorEmailAsync(string email);
        Task<bool> AsignarRolAsync(string email, string role);

    }
}
