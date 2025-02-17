using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto) { 
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get() { 
        List<equipos>listadoEquipos = (from e in _equiposContexto.equipos select e).ToList();

            if (listadoEquipos.Count() == 0)
            { 
                return NotFound();
            }
            return Ok(listadoEquipos);
        }
    }
    
}
