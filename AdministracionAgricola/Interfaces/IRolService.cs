

namespace AdministracionAgricola.Interfaces
{
    public interface IRolService
    {
        Task<bool> CreateRolAsync(string nameRol);
    }
}
