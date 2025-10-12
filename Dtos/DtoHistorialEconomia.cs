namespace API_GestionEconomia.Dtos
{
    public class DtoHistorialEconomia
    {
        public int NumMovimiento { get; set; }
        public string? DescripcionMovimiento { get; set; }
        public decimal IngresoEgreso { get; set; }
        public DateTime FechaMovimiento { get; set; }

    }
}
