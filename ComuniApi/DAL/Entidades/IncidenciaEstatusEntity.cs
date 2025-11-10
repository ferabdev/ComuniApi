namespace ComuniApi.DAL.Entidades
{
    public class IncidenciaEstatusEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<IncidenciaEntity> Incidencias = new List<IncidenciaEntity>();
    }
}
