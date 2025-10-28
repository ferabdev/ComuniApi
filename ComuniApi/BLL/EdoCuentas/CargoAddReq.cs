namespace ComuniApi.BLL.EdoCuentas
{
    public class CargoAddReq
    {
        public int ConceptoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaLimite { get; set; }
    }

    public class PagoAddReq
    {
        public decimal Monto { get; set; }
    }
}
