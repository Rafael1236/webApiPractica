using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
        public IActionResult Get()
        {
            List<equipos> listadoEquipos = (from e in _equiposContexto.equipos select e).ToList();

            if (listadoEquipos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipos);
        }

        [HttpGet]
        [Route("GetByIs/{id}")]
        public IActionResult Get(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                                            where e.id_equipos == id
                                            select e).FirstOrDefault();

            if (equipo ==  null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpGet]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody]equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos 
            //al cual alteraremos alguna propiedad
            equipos? equipoActual = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (equipoActual == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos modificables
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id; 
            equipoActual.anio_compra_id = equipoModificar.anio_compra_id; 
            equipoActual.costo = equipoModificar.costo;

            //Se marca el registro como modificado en el contexto 
            //y se envia la modificacion a la base de datos
            _equiposContexto.Entry(equipoActual).State = EntityState.Modified; 
            _equiposContexto.SaveChanges();
            return Ok(equipoModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id) { 
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if (equipo == null) { return NotFound(); }

            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        
        }
    }

}
