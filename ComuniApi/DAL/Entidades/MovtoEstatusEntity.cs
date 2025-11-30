namespace ComuniApi.DAL.Entidades
{
    public class MovtoEstatusEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public virtual ICollection<EdoCuentaEntity> EdoCuentas { get; set; } = new List<EdoCuentaEntity>();
    }
}
