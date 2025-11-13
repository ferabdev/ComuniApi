namespace ComuniApi.BLL.Reportes
{
    public class ReporteReq
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class ReporteRes : ReporteReq
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
