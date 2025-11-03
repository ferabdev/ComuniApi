namespace ComuniApi.DAL.Entidades
{
    public class EdoCuentaEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaLimite { get; set; }
        public int UsuarioId { get; set; }
        public int? ConceptoId { get; set; }    //Los pagos no vienen con concepto

        public virtual UsuarioEntity Usuario { get; set; } = null!;
        public virtual ConceptoEntity? Concepto { get; set; } = null!;
    }
}
