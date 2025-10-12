using System.ComponentModel.DataAnnotations;

namespace API_GestionEconomia.DataBase.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [StringLength(320)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password_Hash { get; set; }
        [Required]
        public string? CodigoUsuario { get; set; }

    }
}
