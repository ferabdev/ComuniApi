namespace ComuniApi.BLL.EdoCuentas
{
    public class MovtosRes
    {
        public int Id { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public DateOnly FechaLimite { get; set; }
    }
}
