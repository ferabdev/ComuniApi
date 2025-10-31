namespace ComuniApi.BLL.Comunidades
{
    public class UsuariosComunidadRes
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
    }
}
