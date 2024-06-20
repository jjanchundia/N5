using Microsoft.AspNetCore.Mvc;
using MediatR;
using N5.Application.UseCases.Permisos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace N5.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermisoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SolicitarPermiso(SolicitarPermiso.SolicitarPermisoCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("modificarPermiso")]
        public async Task<IActionResult> ModificarPermiso(ModificarPermiso.ModificarPermisoCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPermisos()
        {
            var response = await _mediator.Send(new ObtenerPermiso.ObtenerPermisoRequest());
            return Ok(response);
        }
    }
}