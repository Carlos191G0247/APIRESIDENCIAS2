using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIRESIDENCIAS.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosEnviadosController : ControllerBase
    {
        private readonly ArchivosenviadorRepository repository;
        private readonly ResidentesRepository residentesRepository; 

        private readonly IWebHostEnvironment webHostEnvironment;

        public ArchivosEnviadosController(ArchivosenviadorRepository repository, IWebHostEnvironment webHostEnvironment, ResidentesRepository residentesRepository)
        {
            this.repository = repository;
            this.webHostEnvironment = webHostEnvironment;
            this.residentesRepository = residentesRepository;
        }

        [HttpGet("TareaCordi/{idresidente}/{numtarea}")]
        public IActionResult GetTareaCordi(int idresidente, int numtarea)
        {
            var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea == numtarea && x.IdResidente == idresidente);
            return Ok(entidad.Id);
        }
        [HttpGet("{numTarea}")]
        public IActionResult Getestatus(int numTarea)
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "IdRes");
            if (User.IsInRole("Residente"))
            {
                var userId = id.Value;
                var userIdInt = int.Parse(userId);
                var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea == numTarea && x.IdResidente == userIdInt);

                if (entidad == null)
                {   
                    return Ok("No Entregado");
                }
                var estatuss = entidad.Estatus.ToString();

               

                if (estatuss == "1")
                {
                    estatuss = "Enviado";
                }
                else if (estatuss == "2")
                {
                    estatuss = "Regresado";
                }
                else if (estatuss == "3")
                {
                    estatuss =  "Entregado Tarde";
                }
                else
                {
                    estatuss = "No Entregado";
                }
                return Ok(estatuss);
            }
            else { return BadRequest(); }
            //var entidad = repository.Get(estatus);

        }
        //[HttpGet("{estatus}")]
        //public IActionResult Getestatus(int estatus)
        //{
        //    var entidad = repository.Get(estatus);
        //    if (entidad == null)
        //    {
        //        return NotFound(); 
        //    }
        //    var estatuss = entidad.Estatus.ToString();

        //     if (estatuss == "1")
        //    {
        //        estatuss = "Enviado";
        //    }
        //    else if(estatuss == "2")
        //    {
        //        estatuss = "Regresado";
        //    }
        //    else
        //    {
        //        estatuss = "No Entregado";
        //    }
        //    return Ok(estatuss);
        //}

        [HttpGet("Datos/{numTarea}")]
        public IActionResult GetAllDatos(int numTarea)
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "IdRes");
            if (User.IsInRole("Residente"))
            {
                var userId = id.Value;
                var userIdInt = int.Parse(userId);

                var idRes = repository.Context.Archivosenviados.Include(x => x.IdResidenteNavigation).ToList();
               
               
               
                var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea ==numTarea && x.IdResidente==userIdInt);
                if (entidad != null)
                {
                    return Ok(entidad);
                }
                else
                {
                    return Ok(entidad);
                }
                
            
            }
            else
            {
                return BadRequest("No trajo Ningun Dato");
            }
            
        }
        [HttpPost]
        public IActionResult Post(ArchivosEnviadosDTO dto)
        {
            if (User.IsInRole("Residente"))
            {
                Archivosenviados ae = new()
                {
                    IdResidente = dto.IdResidente,
                    NombreArchivo = dto.NombreArchivo,
                    FechaEnvio = dto.FechaEnvio,
                    NumTarea = dto.NumTarea,
                    Estatus = dto.Estatus,
                };
                repository.Insert(ae);

                return Ok(ae.Id);
            }
            else
            {
                return Unauthorized("No tienes permisos para realizar esta acción.");

            }


        }

        [HttpPost("PDF")]
        public IActionResult Post(PdfDTO dto)
        {
            if (User.IsInRole("Residente"))
            {
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
            else
            {
                return Unauthorized("No tienes permisos para realizar esta acción.");
            }


              
        }
        //rol para cordi
        [HttpPut("{numTarea}/{idresidente}")]
        public IActionResult Put(int numTarea,int idresidente) 
        {

            var id = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (User.IsInRole("Admin")||User.IsInRole("Telma"))
            {
               
                //var entidad = residentesRepository.GetAll().SingleOrDefault(x=>x.IdIniciarSesion==userIdInt) ;
                var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea == numTarea && x.IdResidente == idresidente);
                entidad.Estatus = 2;
                repository.Update(entidad);

            }
            else
            {
                return Unauthorized("No se pudo obtener el Id del usuario");

            }
            return Ok("Operación completada exitosamente");


        }
    
        [HttpDelete("{numTarea}")]
        public IActionResult Delete(int numTarea)
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "IdRes");
            if (User.IsInRole("Residente"))
            {
                var userId = id.Value;
                var userIdInt = int.Parse(userId);
                var entidad = repository.GetAll().FirstOrDefault(x => x.NumTarea == numTarea && x.IdResidente == userIdInt);

                var tarea = repository.Get(entidad.Id);
                if (tarea == null)
                    return BadRequest("No existe o ya se eliminado");

                else
                {
                    repository.Delete(tarea);

                    //borro la imagen
                    var ruta = webHostEnvironment.WebRootPath + $"/pdfs/{entidad.Id}.pdf";
                    System.IO.File.Delete(ruta);

                    return Ok();
                }
            }
            else
            {
                return Unauthorized("No se pudo obtener el Id del usuario");

            }


        }
        //[HttpDelete]
        //public IActionResult Delete(EliminarTareaDTO p)
        //{
        //    var tarea = repository.Get(p.Id);
        //    if (tarea == null)
        //        return BadRequest("No existe o ya se eliminado");

        //    else
        //    {
        //        repository.Delete(tarea);

        //        //borro la imagen
        //        var ruta = webHostEnvironment.WebRootPath + $"/pdfs/{p.Id}.pdf";
        //        System.IO.File.Delete(ruta);

        //        return Ok();
        //    }

        //}

    }
}
