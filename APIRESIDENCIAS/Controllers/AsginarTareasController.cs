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
        [HttpGet("{tareaId}")]
        public IActionResult GetTarea(int tareaId)
        {
            var entidad = repository.Get(tareaId);
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

        [HttpPost("PDF")]
        public IActionResult Post(PdfDTO dto)
        {
            var producto = repository.Get(dto.Id);

            if (producto == null)
            {
                return BadRequest("No existe el producto especificado");
            }

            var ruta = webHostEnvironment.WebRootPath + "/tareasasignadas/";
            System.IO.Directory.CreateDirectory(ruta);

            ruta += dto.Id + ".pdf";

            var file = System.IO.File.Create(ruta);

            byte[] pdf = Convert.FromBase64String(dto.pdfBase64);
            file.Write(pdf, 0, pdf.Length);
            file.Close();

            return Ok();
        }

    }
}
