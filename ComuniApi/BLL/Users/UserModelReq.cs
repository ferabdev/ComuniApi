using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComuniApi.BLL.Users
{
    public class UserModelReq
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int ComunidadId { get; set; }
    }
}
