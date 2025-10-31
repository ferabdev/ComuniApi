namespace ComuniApi.BLL.EdoCuentas
{
    public class CargoAddReq
    {
        public int UsuarioId { get; set; }
        public int ConceptoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaLimite { get; set; }
    }

    public class PagoAddReq
    {
        public DateTime FechaPago { get; set; }
        public int UsuarioId { get; set; }
        public decimal Monto { get; set; }
    }
}
