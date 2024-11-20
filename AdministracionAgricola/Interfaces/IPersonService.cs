namespace AdministracionAgricola.Interfaces
{
    public interface IPersonService
    {
        Task<bool> createPerson(string name, string apellido, int dni, string IdUser);
    }
}
