using CRUD.NCapas.BackEnd.Net.Data.Context;
using CRUD.NCapas.BackEnd.Net.Data.Models;
using CRUD.NCapas.BackEnd.Net.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRUD.NCapas.BackEnd.Net.Service.Implementation
{
    public class PersonaService : IPersonaService
    {
        private readonly DataContext _dataContext;

        public PersonaService(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public List<Persona> GetAll()
        {
            return _dataContext.Persona.ToList();
        }

        public Persona? GetById(int idPersona)
        {
            return _dataContext.Persona.Find(idPersona);
        }

        public Persona Add(Persona persona)
        {
            _dataContext.Persona.Add(persona);
            _dataContext.SaveChanges();

            return persona;
        }

        public bool Update(Persona persona)
        {
            _dataContext.Entry(persona).State = EntityState.Modified;
            _dataContext.SaveChanges();

            return true;
        }

        public bool Delete(int idPersona)
        {
            var persona = _dataContext.Persona.Find(idPersona);

            if (persona == null)
            {
                return false;
            }

            _dataContext.Persona.Remove(persona);
            _dataContext.SaveChanges();

            return true;
        }
    }
}
