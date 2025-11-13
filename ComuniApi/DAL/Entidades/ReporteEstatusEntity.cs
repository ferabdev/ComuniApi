namespace ComuniApi.DAL.Entidades
{
    public class ReporteEstatusEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<ReporteEntity> Reportes = new List<ReporteEntity>();
    }
}
