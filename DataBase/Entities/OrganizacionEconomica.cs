using System.ComponentModel.DataAnnotations;

namespace API_GestionEconomia.DataBase.Entities
{
    public class OrganizacionEconomica
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? NombreOrg { get; set; }
        [Required]
        public string? PasswordOrg_Hash { get; set; }
        [Required]
        public string? CodigoEconomia { get; set; }
        [Required]
        public string? FKCodigoUsuario { get; set; }


    }
}
