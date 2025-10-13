using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_GestionEconomia.DataBase.Entities
{
    public class OrganizacionEconomica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? NombreOrg { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? PasswordOrg_Hash { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? CodigoEconomia { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? FKCodigoUsuario { get; set; }
    }
}
