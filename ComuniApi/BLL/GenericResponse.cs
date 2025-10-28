using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComuniApi.BLL
{
    public class GenericResponse<T>
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public string ExtraInfo { get; set; } = string.Empty;
        public bool Exitoso
        {
            get
            {
                return (int)Status >= 200 && (int)Status < 300;
            }
        }
        public T? Data { get; set; } = default;
    }
}
