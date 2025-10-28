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

        public virtual UsuarioEntity Usuario { get; set; } = null!;
    }
}
