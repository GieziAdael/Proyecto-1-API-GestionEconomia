using System.ComponentModel.DataAnnotations;

namespace API_GestionEconomia.DataBase.Entities
{
    public class Organizador
    {
        public int Id { get; set; }
        [Required]
        [StringLength(5)]
        public string? tipoUsuario { get; set; }
        [Required]
        public string? FKCodigoEconomia { get; set; }

        public int FKIdUsuario { get; set; }
    }
}
