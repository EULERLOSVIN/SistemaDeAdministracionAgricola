using AdministracionAgricola.Models;
using Microsoft.AspNetCore.Identity;

namespace AdministracionAgricola.Interfaces
{
    public interface IListarUsuarioService
    {

        Task<string> ObtenerRolesConTipoAsync(string userId);

        Task<List<UsuarioDetalle>> ListarTodoUsuarios();  // Método para listar usuarios con detalles

        Task<UsuarioDetalle> ObtenerUsuarioPorIdAsync(string id);
    }
}

