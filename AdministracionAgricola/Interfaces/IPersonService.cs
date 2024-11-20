namespace AdministracionAgricola.Interfaces
{
    public interface IPersonService
    {
        //interface
        Task<bool> createPerson(string name, string apellido, int dni, string IdUser);
    }
}
