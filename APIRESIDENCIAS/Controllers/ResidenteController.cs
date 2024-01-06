using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Linq;
using System.Text.RegularExpressions;
namespace APIRESIDENCIAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidenteController : ControllerBase
    {
        private readonly ResidentesRepository repository;
        private readonly  CarreraRepository carreraRepository;

        public ResidenteController( ResidentesRepository repository ,CarreraRepository carreraRepository)
        {
                this.repository = repository;
                this.carreraRepository = carreraRepository;
        }
        //Rol como admin
        [HttpGet("Filtro/{Fecha}/{check1}/{check2}/{numtarea}")]
        public IActionResult GetFiltro(int Fecha, bool check1 ,bool check2,int numtarea )
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "IdCarrera");
            if (User.IsInRole("Admin"))
            {
                
                var userId = id.Value;
                var userIdInt = int.Parse(userId);
                var trarCarrera = carreraRepository.Get(userIdInt);

                
                var entidad = repository.Context.Residentes.Include(x=>x.Archivosenviados).Include(x=>x.IdIniciarSesionNavigation).Where(x => x.IdCarrera == trarCarrera.Id && x.Fecha.Year ==Fecha).ToList();

                if (check1 == true && check2 ==false)
                {
                    var residentesConArchivos = entidad
                                 .Where(residente => residente.Archivosenviados != null && residente.Archivosenviados.Any(x => x.NumTarea == numtarea))
                                 .ToList();
                    return Ok(residentesConArchivos);
                }
                if (check2 == true && check1 == false)
                {
                    var residentesSinTareas = entidad
                   .Where(residente => residente.Archivosenviados == null || !residente.Archivosenviados.Any(archivo => archivo.NumTarea == numtarea))
                   .ToList();
                    return Ok(residentesSinTareas);
                }
                if (check1 == false && check2 == false)
                {
                    return Ok(entidad);
                }
                if (check1 == true && check2 == true)
                {
                    return Ok (entidad);
                }


            
                return Ok(entidad);
            
            }
            else
            {
                return Ok("ok");
            }


        }

    //     foreach (var residente in entidad)
    //            {
    //                repository.Context.Entry(residente).Collection(e => e.Archivosenviados).Load();
    //    residente.Archivosenviados = residente.Archivosenviados.Where(a => a.NumTarea == 2).ToList();
    //}

    //[HttpGet("Filtro/{Fecha}/{check1}")]
    //public IActionResult GetFiltro(int Fecha, bool check1)
    //{
    //    var id = User.Claims.FirstOrDefault(x => x.Type == "IdCarrera");
    //    if (User.IsInRole("Admin"))
    //    {
    //        // de residentes
    //        var userId = id.Value;
    //        var userIdInt = int.Parse(userId);
    //        var trarCarrera = carreraRepository.Get(userIdInt);



    //        var entidad = repository.GetAll().Where(x => x.IdCarrera == trarCarrera.Id && x.Fecha.Year == Fecha).ToList();

    //        foreach (var residente in entidad)
    //        {
    //            repository.Context.Entry(residente).Collection(e => e.Archivosenviados).Load();
    //            residente.Archivosenviados = residente.Archivosenviados.Where(a => a.NumTarea > 0).ToList();
    //        }
    //        return Ok(entidad);
    //    }
    //    else
    //    {
    //        return Ok("ok");
    //    }


    //}
    [HttpGet("nombre")]
        public IActionResult Get()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (User.IsInRole("Admin"))
            {
                // de residentes
                var userId = id.Value;
                var userIdInt = int.Parse(userId);
                var entidad = repository.GetAll().SingleOrDefault(x => x.IdIniciarSesion==userIdInt);
                return Ok(entidad.NombreCompleto);
            }
            else
            {
                return Ok("ok");
            }


        }
        [HttpGet("fecha")]
        public IActionResult GetAll()
        {
            //Role Como Coordi
            if (User.IsInRole("Admin"))
            {
                var entidad = repository.GetAll().Select(x => x.Fecha.Year).Distinct();
                return Ok(entidad); 
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entidad = repository.Get(id);
            return Ok(entidad?.NombreCompleto);
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
            if (string.IsNullOrWhiteSpace(dto.NombreCompleto) || dto.NombreCompleto.Trim().Length < 2 || dto.NombreCompleto.Trim().Length > 60)
            {
                return BadRequest("El nombre completo debe tener entre 2 y 60 caracteres.");
            }
            if (!Regex.IsMatch(dto.NombreCompleto, "^[a-zA-Z ]+$"))
            {
                return BadRequest("El nombre completo solo debe contener letras y espacios.");
            }
            if (string.IsNullOrEmpty(dto.NumControl) || dto.NumControl.Trim().Length < 8 || dto.NumControl.Trim().Length > 10)
            {
                return BadRequest("El número de control debe tener entre 8 y 10 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(dto.Cooasesor) || dto.Cooasesor.Trim().Length < 2 || dto.Cooasesor.Trim().Length > 60)
            {
                return BadRequest("El nombre del asesor debe tener entre 2 y 60 caracteres.");
            }
            if (!Regex.IsMatch(dto.NombreCompleto, "^[a-zA-Z ]+$"))
            {
                return BadRequest("El nombre del asesor solo debe contener letras y espacios.");
            }

            if (string.IsNullOrEmpty(dto.Contrasena) || dto.Contrasena.Trim().Length < 8 || dto.Contrasena.Trim().Length > 10)
            {
                return BadRequest("La contraseña debe tener entre 8 y 10 caracteres.");
            }

            if (!Regex.IsMatch(dto.Contrasena, "^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]+$"))
            {
                return BadRequest("La contraseña debe contener al menos una letra, un número y un carácter especial.");
            }


            // Validamos
            if (ModelState.IsValid)
            {
                Residentes re = new()
                {
                    NombreCompleto = dto.NombreCompleto,
                    Cooasesor = dto.Cooasesor,
                    Fecha = dto.Fecha,
                    IdCarrera = dto.Carrera
                    
                };
                Inciarsesion isa = new()
                {
                    Contrasena = dto.Contrasena,
                    Numcontrol = dto.NumControl
                };

                //Carrera ca = new()
                //{
                //    NombreCarrera = dto.Carrera
                //};
                re.IdIniciarSesionNavigation = isa;
                //re.IdCarreraNavigation = ca;
                repository.Insert(re);
            }
            return Ok();
        }
    
    }

}
