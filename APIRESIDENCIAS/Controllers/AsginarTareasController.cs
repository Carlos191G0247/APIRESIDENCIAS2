using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRESIDENCIAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsginarTareasController : ControllerBase
    {
        private readonly AsignartareasRepository repository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AsginarTareasController(AsignartareasRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            this.repository = repository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var entidad = repository.GetAll();
            return Ok(entidad);

        }

        [HttpPost]
        public IActionResult Post(AsignarTareasDTO dto)
        {

            Asignaciontareas at = new()
            {
                Idcoordinador = dto.Idcoordinador,
                NombreTarea = dto.NombreTarea,
                Fecha = dto.Fecha,
                Intruccion = dto.Intruccion,
                NumTarea = dto.NumTarea,
            };
            repository.Insert(at);

            return Ok(at.Id);

        }
    }
}
