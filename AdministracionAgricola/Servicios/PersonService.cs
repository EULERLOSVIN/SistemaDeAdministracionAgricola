using AdministracionAgricola.Interfaces;
using AdministracionAgricola.Models;

namespace AdministracionAgricola.Servicios
{
    public class PersonService : IPersonService
    {
        private readonly AdministracionCultivosContext _context;

        public PersonService(AdministracionCultivosContext context)
        {
            _context = context;
        }

        public async Task<bool> createPerson(string name, string apellido, int dni, string IdUser)
        {
            var persona = new Persona { Nombre = name, Apellido = apellido, Dni = dni, PkAspNetUsers = IdUser };
            try
            {
                await _context.AddAsync(persona);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
    }
}
