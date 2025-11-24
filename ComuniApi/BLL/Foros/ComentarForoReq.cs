namespace ComuniApi.BLL.Foros
{
    public class ComentarForoReq
    {
        public int ForoId { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
