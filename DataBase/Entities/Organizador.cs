using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_GestionEconomia.DataBase.Entities
{
    public class Organizador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(5)]
        [Column(TypeName = "varchar(5)")]
        public string? tipoUsuario { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? FKCodigoEconomia { get; set; }

        public int FKIdUsuario { get; set; }
    }
}
