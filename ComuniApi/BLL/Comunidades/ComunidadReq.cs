namespace ComuniApi.BLL.Comunidades
{
    public class ComunidadReq
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int MaxUsers { get; set; } = 0;
    }
}
