using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRESIDENCIAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        private readonly SolicitudesRepository repository;


        public SolicitudesController(SolicitudesRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var entidad = repository.GetAll();
            return Ok(entidad);
        }
        [HttpPost]
        public IActionResult Post(SolicitudesDTO dto)
        {
            // Validamos
            if (ModelState.IsValid)
            {
                Solicitudesalumnos soli = new()
                {
                    NombreCompleto = dto.NombreCompleto,
                    Correo = dto.Correo,
                    NumeroDeControl = dto.NumeroDeControl,
                    Semestre = dto.Semestre,
                    Grupo = dto.Grupo,
                    Cooasesor = dto.Cooasesor,
                    Carrera = dto.Carrera,
                };

                repository.Insert(soli);
            }
            return Ok();
        }
        [HttpPut]
        public IActionResult Put(SolicitudesDTO dto)
        {

            if (ModelState.IsValid)
            {
                var solicitudes = repository.Get(dto.Idsolicitudesalumnos);

                if (solicitudes != null)
                {
                    solicitudes.NombreCompleto = dto.NombreCompleto;
                    solicitudes.Correo = dto.Correo;
                    solicitudes.NumeroDeControl = dto.NumeroDeControl;
                    solicitudes.Semestre = dto.Semestre;
                    solicitudes.Grupo = dto.Grupo;
                    solicitudes.Cooasesor = dto.Cooasesor;
                    solicitudes.Carrera = dto.Carrera;
                    repository.Update(solicitudes);

                }
            }
            return Ok();

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var entidad = repository.Get(id);
                if (entidad != null)
                {
                    repository.Delete(entidad);
                }
                else
                {
                    return NotFound();

                }
                return Ok();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }



        }

    }
}
