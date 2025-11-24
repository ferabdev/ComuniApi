namespace ComuniApi.DAL.Entidades
{
    public class ForoComentarioEntity
    {
        public Guid Id { get; set; }
        public int ForoId { get; set; }
        public int UsuarioId { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }

        public virtual ForoEntity Foro { get; set; } = null!;
        public virtual UsuarioEntity Usuario { get; set; } = null!;
    }
}
