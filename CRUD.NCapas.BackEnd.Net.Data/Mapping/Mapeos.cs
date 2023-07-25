using AutoMapper;
using CRUD.NCapas.BackEnd.Net.Data.Models;
using CRUD.NCapas.BackEnd.Net.Domain.To.ViewModels;

namespace CRUD.NCapas.BackEnd.Net.Data.Mapping
{
    public class Mapeos : Profile
    {
        public Mapeos()
        {
            #region Persona 

            CreateMap<Persona, PersonaViewModel>()
                 .ForMember(m => m.IdPersona, map => map.MapFrom(vm => vm.IdPersona))
                 .ForMember(m => m.Nombres, map => map.MapFrom(vm => vm.Nombres))
                 .ForMember(m => m.Apellidos, map => map.MapFrom(vm => vm.Apellidos))
                 .ForMember(m => m.NoDocumento, map => map.MapFrom(vm => vm.NoDocumento))
                 .ReverseMap();

            #endregion
        }
    }
}
