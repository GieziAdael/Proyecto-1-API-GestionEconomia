using System.ComponentModel.DataAnnotations;

namespace API_GestionEconomia.Dtos
{
    public class DtoOrgEconm
    {
        [Required]
        public string? NombreOrg { get; set; }
        [Required]
        public string? PasswordOrg { get; set; }
        [Required]
        public string? CodigoUsuario { get; set; }
    }
}
