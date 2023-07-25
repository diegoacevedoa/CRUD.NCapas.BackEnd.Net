using CRUD.NCapas.BackEnd.Net.Data.Models;

namespace CRUD.NCapas.BackEnd.Net.Service.Interface
{
    public interface IPersonaService
    {
        List<Persona> GetAll();

        Persona? GetById(int idPersona);

        Persona Add(Persona persona);

        bool Update(Persona persona);

        bool Delete(int idPersona);
    }
}
