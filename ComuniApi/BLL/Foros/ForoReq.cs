namespace ComuniApi.BLL.Foros
{
    public class ForoReq
    {
        public string Nombre { get; set; } = string.Empty;
        public List<ForoComentarioReq> Comentarios { get; set; } = new();
    }

    public class ForoComentarioReq
    {
        public string Mensaje { get; set; } = string.Empty;
    }
}
