using System.ComponentModel.DataAnnotations;

namespace API_GestionEconomia.Dtos
{
    public class DtoAccederOrgEconm
    {
        [Required]
        public string? NombreOrg { get; set; }
        [Required]
        public string? PasswordOrg { get; set; }
    }
}
