using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace APIRESIDENCIAS.Controllers
{
    [Authorize]
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
        
        //Rol para Admin
        [HttpGet]
        public IActionResult Get()
        {
            if (User.IsInRole("Admin"))
            {
                var entidad = repository.GetAll().OrderBy(x=>x.NumTarea);
                return Ok(entidad);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("vertarea/{tareaId}")]
        public IActionResult GetTareaespecifica(int tareaId)
        {
            
            if (User.IsInRole("Residente"))
            {
                var entidad = repository.GetAll().SingleOrDefault(x=>x.NumTarea == tareaId);
                return Ok(entidad.Id);

            }
            else
            {
                return Unauthorized("No se pudo obtener el Id del usuario");

            }
        }
        [HttpGet("{tareaId}")]
        public IActionResult GetTarea(int tareaId)
        {
            //checar si se usa en admin??
            if (User.IsInRole("Admin"))
            {
                var entidad = repository.Get(tareaId);
                return Ok(entidad);

            }
            else {
                var entidad = repository.GetAll().SingleOrDefault(x=>x.NumTarea ==tareaId);
                return Ok(entidad);

            }
        }

        [HttpPost]
        public IActionResult Post(AsignarTareasDTO dto)
        {
            var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea == dto.NumTarea);

            if (entidad != null)
            {
                return BadRequest("Ya existe una tarea con el mismo número. Por favor, elige otro número de tarea o edita la tarea.");
            }


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
     
        [HttpPut("{tareaId}")]
        public IActionResult Put(int tareaId, EditarDTO dto)
        {
            try
            {
                var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea == dto.NumTarea);

                if (entidad != null)
                {
                    return BadRequest("Ya existe una tarea con el mismo número. Por favor, elige otro número de tarea ");
                }
                // Verifica si el usuario tiene el rol adecuado
                if (!User.IsInRole("Admin"))
                {
                    return Unauthorized("No tienes permisos para realizar esta acción.");
                }

                // Obtiene la tarea existente por su id
                var tareaExistente = repository.Get(tareaId);

                // Verifica si la tarea existe
                if (tareaExistente == null)
                {
                    return NotFound($"No se encontró la tarea ");
                }

                // Realiza una comparación de los valores actuales y los nuevos valores
                bool cambiosRealizados =
                    tareaExistente.Idcoordinador != dto.Idcoordinador ||
                    tareaExistente.NombreTarea != dto.NombreTarea ||
                    tareaExistente.Fecha != dto.Fecha ||
                    tareaExistente.Intruccion != dto.Intruccion ||
                    tareaExistente.NumTarea != dto.NumTarea;

                // Si no se realizaron cambios, devuelve un mensaje indicando que no se editó nada
                if (!cambiosRealizados)
                {
                    return BadRequest("No realizaste ningun cambio");
                   
                }

                // Actualiza los campos de la tarea con los valores proporcionados en el DTO
                tareaExistente.Idcoordinador = dto.Idcoordinador;
                tareaExistente.NombreTarea = dto.NombreTarea;
                tareaExistente.Fecha = dto.Fecha;
                tareaExistente.Intruccion = dto.Intruccion;
                tareaExistente.NumTarea = dto.NumTarea;

                // Llama al método Update del repositorio para aplicar los cambios
                repository.Update(tareaExistente);

                return Ok();
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción y devuelve un mensaje de error
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        

        //rol para admin
        [HttpDelete("{numTarea}")]
        public IActionResult Delete(int numTarea)
        {
            if (User.IsInRole("Admin"))
            {
               

                var tarea = repository.Get(numTarea);
                if (tarea == null)
                    return BadRequest("No existe o ya se eliminado");

                else
                {
                    repository.Delete(tarea);

                    //borro la imagen
                    var ruta = webHostEnvironment.WebRootPath + $"/pdfs/{numTarea}.pdf";
                    System.IO.File.Delete(ruta);

                    return Ok();
                }
            }
            else
            {
                return Unauthorized("No se pudo obtener el Id del usuario");

            }


        }

        [HttpDelete("EliminarPDF/{numTarea}")]
        public IActionResult DeletePDF(int numTarea)
        {
            if (User.IsInRole("Admin"))
            {

                var ruta = webHostEnvironment.WebRootPath + $"/tareasasignadas/{numTarea}.pdf";

                if (ruta == null)
                {
                    return Ok();
                }
                else
                {
                    System.IO.File.Delete(ruta);
                    return Ok();
                }
               
            }
            else
            {
                return Unauthorized("No se pudo obtener el Id del usuario");

            }


        }
    }
}
