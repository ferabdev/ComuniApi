using ComuniApi.DAL;
using Microsoft.EntityFrameworkCore;

namespace ComuniApi.BLL.Comunidades
{
    public class ComunidadService
    {
        private readonly ComuniContext _context;

        public ComunidadService(ComuniContext context)
        {
            _context = context;
        }

        public async Task<GenericResponse<List<UsuariosComunidadRes>>> ObtenerUsuariosComunidad(int comunidadId)
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Where(u => u.ComunidadId == comunidadId)
                    .Select(u => new UsuariosComunidadRes
                    {
                        Id = u.Id,
                        Username = u.Username,
                        NombreCompleto = u.NombreCompleto,
                        Saldo = u.EdoCuentas.Sum(e => e.Monto)
                    })
                    .ToListAsync();

                return new GenericResponse<List<UsuariosComunidadRes>>
                {
                    Status = System.Net.HttpStatusCode.OK,
                    Message = "Usuarios obtenidos exitosamente.",
                    Data = usuarios
                };
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrar el error o lanzar una excepción personalizada)
                throw new ApplicationException("Error al obtener los usuarios de la comunidad.", ex);
            }
        }
    }
}
