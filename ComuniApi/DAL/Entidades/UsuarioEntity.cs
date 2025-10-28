using System.ComponentModel.DataAnnotations;

namespace ComuniApi.DAL.Entidades
{
    public class UsuarioEntity
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string NombreCompleto { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<EdoCuentaEntity> EdoCuentas { get; set; } = new List<EdoCuentaEntity>();
    }
}
