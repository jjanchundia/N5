using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Application.UseCases.TipoPermisos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace N5.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoPermisoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TipoPermisoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<TipoPermisoController>
        [HttpGet]
        public async Task<IActionResult> ObtenerTipoPermisos()
        {
            var response = await _mediator.Send(new ObtenerTiposPermisos.ObtenerTiposPermisosRequest());
            return Ok(response);
        }

        [HttpGet("obtenerTipoPermisoPorId")]
        public async Task<IActionResult> ObtenerPermisoPorId(int id)
        {
            var response = await _mediator.Send(new ObtenerTipoPermisoPorId.ObtenerTipoPermisoPorIdRequest() { Id = id });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CrearTipoPermiso(CrearTipoPermiso.CrearTipoPermisoCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("modificarTipoPermiso")]
        public async Task<IActionResult> ModificarTipoPermiso(ModificarTipoPermiso.ModificarTipoPermisoCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}