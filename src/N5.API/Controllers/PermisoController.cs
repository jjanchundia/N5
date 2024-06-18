using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace N5.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SolicitarPermiso()
        {
            //var response = await _mediator.Send(new GetlAllLibrosQuery());
            return Ok();
        }
        
        [HttpPut]
        public async Task<IActionResult> ModificarPermiso()
        {
            //var response = await _mediator.Send(new GetlAllLibrosQuery());
            return Ok();
        }

        // GET: api/<PermisoController>
        [HttpGet]
        public async Task<IActionResult> ObtenerPermisos()
        {
            //var response = await _mediator.Send(new GetlAllLibrosQuery());
            return Ok();
        }
    }
}
