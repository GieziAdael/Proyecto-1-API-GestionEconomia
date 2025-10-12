using System.ComponentModel.DataAnnotations;

namespace API_GestionEconomia.Dtos
{
    public class DtoUser
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
