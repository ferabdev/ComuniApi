namespace ComuniApi.BLL.Foros
{
    public class ForoRes : ForoReq
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }

        public new List<ForoComentarioRes> Comentarios { get; set; } = [];
    }

    public class ForoComentarioRes : ForoComentarioReq
    {
        public Guid Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
