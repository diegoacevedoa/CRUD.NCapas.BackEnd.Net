using System.ComponentModel.DataAnnotations;

namespace CRUD.NCapas.BackEnd.Net.Domain.To.ViewModels
{
    public class PersonaViewModel
    {
        public int IdPersona { get; set; }


        [Display(Name = "NoDocumento")]
        [StringLength(50)]
        public string NoDocumento { get; set; }

        [Display(Name = "Nombres")]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(100)]
        public string Apellidos { get; set; }
    }
}
