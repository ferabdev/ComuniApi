namespace ComuniApi.DAL.Entidades
{
    public class ForoEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int ComunidadId { get; set; }
        public bool Votacion { get; set; }
        public int UsuarioId { get; set; }

        public virtual ComunidadEntity Comunidad { get; set; } = null!;
        public virtual UsuarioEntity Usuario { get; set; } = null!;
        public virtual ICollection<ForoComentarioEntity> Comentarios { get; set; } = new List<ForoComentarioEntity>();
        public virtual ICollection<ForoVotacionOpcionEntity> Opciones { get; set; } = new List<ForoVotacionOpcionEntity>();
        public virtual ICollection<ForoVotoUsuarioEntity> VotosUsuarios { get; set; } = new List<ForoVotoUsuarioEntity>();
    }
}
