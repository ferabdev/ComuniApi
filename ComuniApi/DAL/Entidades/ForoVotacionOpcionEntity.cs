namespace ComuniApi.DAL.Entidades
{
    public class ForoVotacionOpcionEntity
    {
        public int Id { get; set; }
        public int ForoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Votos { get; set; }
        public virtual ForoEntity Foro { get; set; } = null!;
        public virtual ICollection<ForoVotoUsuarioEntity> VotosUsuarios { get; set; } = new List<ForoVotoUsuarioEntity>();
    }
}
