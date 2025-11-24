namespace ComuniApi.DAL.Entidades
{
    public class ForoEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int ComunidadId { get; set; }

        public virtual ComunidadEntity Comunidad { get; set; } = null!;
        public virtual ICollection<ForoComentarioEntity> Comentarios { get; set; } = new List<ForoComentarioEntity>();
    }
}
