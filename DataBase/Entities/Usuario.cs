using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_GestionEconomia.DataBase.Entities
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(320)]
        [EmailAddress]
        [Column(TypeName = "varchar(320)")]
        public string? Email { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? Password_Hash { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? CodigoUsuario { get; set; }
    }
}
