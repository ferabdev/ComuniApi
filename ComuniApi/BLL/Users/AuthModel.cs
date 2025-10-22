using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComuniApi.BLL.Users
{
    public class AuthReq
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthRes
    {
        public UserModel? User { get; set; } = null;
        public string Token { get; set; } = string.Empty;
    }
}
