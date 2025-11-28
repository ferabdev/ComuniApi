namespace ComuniApi.DAL.Entidades
{
    public class ForoVotoUsuarioEntity
    {
        public int ForoId { get; set; }
        public int UsuarioId { get; set; }
        public int OpcionId { get; set; }

        public virtual ForoEntity Foro { get; set; } = null!;
        public virtual UsuarioEntity Usuario { get; set; } = null!;
        public virtual ForoVotacionOpcionEntity Opcion { get; set; } = null!;
    }
}
