namespace ComuniApi.DAL.Entidades
{
    public class EdoCuentaEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaLimite { get; set; }
        public int UsuarioId { get; set; }
        public int? ConceptoId { get; set; }    //Los pagos no vienen con concepto
        public int EstatusId { get; set; } = 2;

        public virtual UsuarioEntity Usuario { get; set; } = null!;
        public virtual ConceptoEntity? Concepto { get; set; } = null!;
        public virtual MovtoEstatusEntity Estatus { get; set; } = null!;
    }
}
