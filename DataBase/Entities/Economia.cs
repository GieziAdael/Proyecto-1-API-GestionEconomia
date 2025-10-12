using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API_GestionEconomia.DataBase.Entities
{
    public class Economia
    {
        public int Id { get; set; }
        [Required]
        public int NumMovimiento { get; set; }
        [Required]
        [StringLength(200)]
        public string? DescripcionMovimiento { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal IngresoEgreso { get; set; }
        [Required]
        public DateTime FechaMovimiento { get; set; }
        [Required]
        public string? FKCodigoEconomia { get; set; }
    }
}
