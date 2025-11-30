namespace ComuniApi.BLL.EdoCuentas
{
    public class MovtosRes
    {
        public int Id { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public DateOnly FechaLimite { get; set; }
    }
}
