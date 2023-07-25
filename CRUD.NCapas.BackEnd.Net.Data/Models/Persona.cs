using System.ComponentModel.DataAnnotations;

namespace CRUD.NCapas.BackEnd.Net.Data.Models
{
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required]
        [StringLength(50)]
        public string NoDocumento { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }
    }
}
