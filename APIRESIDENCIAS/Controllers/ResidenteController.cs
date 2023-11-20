using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRESIDENCIAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidenteController : ControllerBase
    {
        private readonly ResidentesRepository repository;

        public ResidenteController( ResidentesRepository repository)
        {
                this.repository = repository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var entidad = repository.GetAll();
            return Ok(entidad);
            ////var residentes = repository.GetAll();

            //if (residentes != null)
            //{
            //    //GetById
            //    var residentesDTOs = residentes.Select(residente => new ResidenteDTO
            //    {
            //        NombreCompleto = residente.NombreCompleto,
            //        Semestre = residente.Semestre,
            //        Grupo = residente.Grupo,
            //        Cooasesor = residente.Cooasesor,
            //        Carrera =residente.?.NombreCarrera,
            //        Correo = residente.IdIniciarSesionNavigation?.Correo,
            //        NumControl = residente.IdIniciarSesionNavigation?.Numcontrol
            //    }).ToList();

            //    return Ok(residentesDTOs);
            //}
            //else
            //{
            //    return NotFound();
            //}
        }
        [HttpPost]
        public IActionResult Post(ResidenteDTO dto)
        {
            // Validamos
            if (ModelState.IsValid)
            {
                Residentes re = new()
                {
                    NombreCompleto = dto.NombreCompleto,
                    Cooasesor = dto.Cooasesor,
                    Fecha = dto.Fecha,
                    
                };
                Inciarsesion isa = new()
                {
                    Contrasena = dto.Contrasena,
                    Numcontrol = dto.NumControl
                };

                Carrera ca = new()
                {
                    NombreCarrera = dto.Carrera
                };
                re.IdIniciarSesionNavigation = isa;
                re.IdCarreraNavigation = ca;
                repository.Insert(re);
            }
            return Ok();
        }
    
    }

}
