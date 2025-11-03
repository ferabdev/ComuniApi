namespace ComuniApi.DAL.Entidades
{
    public class ComunidadEntity
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int MaxUsers { get; set; }
        public string CodigoComunidad { get; set; }

        public virtual ICollection<UsuarioEntity> Usuarios { get; set; } = new List<UsuarioEntity>();
    }
}
