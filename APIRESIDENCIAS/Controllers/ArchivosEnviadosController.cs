using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRESIDENCIAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosEnviadosController : ControllerBase
    {
        private readonly ArchivosenviadorRepository repository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArchivosEnviadosController(ArchivosenviadorRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            this.repository = repository;
            this.webHostEnvironment = webHostEnvironment;

        }
        [HttpGet("todo/{id}")]
        public IActionResult Get(int id)
        {
            var entidad = repository.Get(id);
            return Ok(entidad);

        }
        [HttpGet("{estatus}")]
        public IActionResult Getestatus(int estatus)
        {
            var entidad = repository.Get(estatus);
            if (entidad == null)
            {
                return NotFound(); 
            }
            var estatuss = entidad.Estatus.ToString();

             if (estatuss == "1")
            {
                estatuss = "Enviado";
            }
            else if(estatuss == "2")
            {
                estatuss = "Regresado";
            }
            else
            {
                estatuss = "No Entregado";
            }
            return Ok(estatuss);
        }

        [HttpGet("{numTarea}/{idResidente}")]
        public IActionResult Getnumtarea(int numTarea, int idResidente)
        {
            var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea ==numTarea && x.IdResidente==idResidente);
            if (entidad == null)
            {
                return NotFound("No entregado"); 
            }
            return Ok(entidad.Id);
        }
        [HttpPost]
        public IActionResult Post(ArchivosEnviadosDTO dto)
        {

            Archivosenviados ae = new()
            {
                IdResidente = dto.IdResidente,
                NombreArchivo = dto.NombreArchivo,
                FechaEnvio = dto.FechaEnvio,
                NumTarea = dto.NumTarea,
                Estatus = 1,
            };
            repository.Insert(ae);

            return Ok(ae.Id);

        }

        [HttpPost("PDF")]
        public IActionResult Post(PdfDTO dto)
        {
            // Validar


            var producto = repository.Get(dto.Id);

            if (producto == null)
            {
                return BadRequest("No existe el producto especificado");
            }

            var ruta = webHostEnvironment.WebRootPath + "/pdfs/";
            System.IO.Directory.CreateDirectory(ruta);

            ruta += dto.Id + ".pdf";

            var file = System.IO.File.Create(ruta);

            byte[] pdf = Convert.FromBase64String(dto.pdfBase64);
            file.Write(pdf, 0, pdf.Length);
            file.Close();

            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete(EliminarTareaDTO p)
        {
            var tarea = repository.Get(p.Id);
            if (tarea == null)
                return BadRequest("No existe o ya se eliminado");

            else
            {
                repository.Delete(tarea);

                //borro la imagen
                var ruta = webHostEnvironment.WebRootPath + $"/pdfs/{p.Id}.pdf";
                System.IO.File.Delete(ruta);

                return Ok();
            }

        }

    }
}
