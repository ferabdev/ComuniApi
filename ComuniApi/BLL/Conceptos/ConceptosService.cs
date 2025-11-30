using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ComuniApi.BLL.Conceptos
{
    public class ConceptosService
    {
        private readonly ComuniContext _context;

        public ConceptosService(ComuniContext context)
        {
            _context = context;
        }

        public async Task<GenericResponse<List<ConceptoRes>>> ObtenerConceptos()
        {
            try
            {
                var res = await _context.Conceptos
                    .Select(c => new ConceptoRes
                    {
                        Id = c.Id,
                        Descripcion = c.Descripcion
                    }).ToListAsync();
                return new GenericResponse<List<ConceptoRes>>
                {
                    Status = System.Net.HttpStatusCode.OK,
                    Message = "Conceptos obtenidos exitosamente.",
                    Data = res
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<ConceptoRes>>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = "Error al obtener los conceptos.",
                    ExtraInfo = ex.Message
                };
            }
        }
    }
}
