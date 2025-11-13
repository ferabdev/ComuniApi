using ComuniApi.BLL.Users;

namespace ComuniApi.DAL.Entidades
{
    public class ReporteEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int EstatusId { get; set; }

        public UsuarioEntity Usuario { get; set; } = null!;
        public ReporteEstatusEntity Estatus { get; set; } = null!;
    }
}
