using System.ComponentModel.DataAnnotations;

namespace ComuniApi.DAL.Entidades
{
    public class UsuarioEntity
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public int RolId { get; set; } = 1;
        public int ComunidadId { get; set; } = 1;

        public virtual RolEntity Rol { get; set; } = null!;
        public virtual ComunidadEntity Comunidad { get; set; } = null!;
        public virtual ICollection<EdoCuentaEntity> EdoCuentas { get; set; } = new List<EdoCuentaEntity>();
    }
}
