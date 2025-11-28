namespace ComuniApi.BLL.Foros
{
    public class VotacionReq
    {
        public string Asunto { get; set; } = string.Empty;
        public List<string> Opciones { get; set; } = new();
    }

    public class VotacionRes : VotacionReq
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public new List<VotacionOpcionRes> Opciones { get; set; } = new();
        public string Usuario { get; set; } = string.Empty;
    }

    public class VotacionOpcionRes
    {
        public int OpcionId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Votos { get; set; }
    }

    public class VotarReq
    {
        public int VotacionId { get; set; }
        public int OpcionId { get; set; }
    }
}
