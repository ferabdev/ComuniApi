namespace ComuniApi.DAL.Entidades
{
    public class ConceptoEntity
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public virtual ICollection<EdoCuentaEntity> EdoCuentas { get; set; } = new List<EdoCuentaEntity>();
    }
}
