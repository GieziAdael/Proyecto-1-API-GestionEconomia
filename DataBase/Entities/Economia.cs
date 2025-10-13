using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_GestionEconomia.DataBase.Entities
{
    public class Economia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int NumMovimiento { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string? DescripcionMovimiento { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal IngresoEgreso { get; set; }

        [Required]
        public DateTime FechaMovimiento { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? FKCodigoEconomia { get; set; }
    }
}
