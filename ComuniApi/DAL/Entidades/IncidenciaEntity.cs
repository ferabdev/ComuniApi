namespace ComuniApi.DAL.Entidades
{
    public class IncidenciaEntity
    {
        public int Id { get; set; }
        public int ComunidadId { get; set; }
        public int UserId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int EstatusId { get; set; }

        public UsuarioEntity Usuario { get; set; } = null!;
        public ComunidadEntity Comunidad { get; set; } = null!;
        public IncidenciaEstatusEntity Estatus { get; set; } = null!;
    }
}
