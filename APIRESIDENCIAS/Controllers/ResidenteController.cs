using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
namespace APIRESIDENCIAS.Controllers
{
    [Authorize]
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
                    //var residentesConArchivos = entidad
                    //             .Where(residente => residente.Archivosenviados != null && residente.Archivosenviados.Any(x => x.NumTarea == numtarea && x.Estatus == 1 || x.NumTarea == numtarea && x.Estatus == 3))
                    //             .ToList();
                    //return Ok(residentesConArchivos);


                    // var residentesConArchivos = entidad
                    //.Where(residente => residente.Archivosenviados != null && residente.Archivosenviados.Any(x => x.NumTarea == numtarea && x.Estatus == 1 || x.NumTarea == numtarea && x.Estatus == 3))
                    //.SelectMany(residente => residente.Archivosenviados.Where(x => x.NumTarea == numtarea && (x.Estatus == 1 || x.Estatus == 3)))
                    //.ToList();

                    // return Ok(residentesConArchivos);
                    var residentesConArchivos = entidad
                        .Where(residente => residente.Archivosenviados != null && residente.Archivosenviados.Any(x => x.NumTarea == numtarea && (x.Estatus == 1 || x.Estatus == 3)))
                        .Select(residente => new
                        {
                            Archivosenviados = residente.Archivosenviados
                                .Where(x => x.NumTarea == numtarea && (x.Estatus == 1 || x.Estatus == 3))
                                .ToList(),
                            residente.Cooasesor,
                            residente.Fecha,
                            residente.Id,
                            residente.IdCarrera,
                            residente.IdCarreraNavigation,
                            residente.IdIniciarSesion,
                            residente.IdIniciarSesionNavigation,
                            residente.NombreCompleto
                        })
                        .ToList();

                    return Ok(residentesConArchivos);

                }
                if (check2 == true && check1 == false)
                {
                    var residentesSinTareas = entidad
                   .Where(residente => residente.Archivosenviados == null || !residente.Archivosenviados.Any(archivo => archivo.NumTarea == numtarea && archivo.Estatus == 1 || archivo.NumTarea == numtarea && archivo.Estatus == 3))
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


        [HttpGet("FiltroTelma/{Fecha}/{check1}/{check2}/{numtarea}/{carrera}")]
        public IActionResult GetFiltroTelma(int Fecha, bool check1, bool check2, int numtarea, int carrera)
        {
           
            if (User.IsInRole("Telma"))
            {
                List<Residentes> entidad;
                if (carrera == 12)
                {
                     entidad = repository.Context.Residentes.Include(x => x.Archivosenviados).Include(x => x.IdIniciarSesionNavigation).Where(x=>x.Fecha.Year == Fecha).ToList();

                }
                else
                {
                     entidad = repository.Context.Residentes.Include(x => x.Archivosenviados).Include(x => x.IdIniciarSesionNavigation).Where(x => x.IdCarrera == carrera && x.Fecha.Year == Fecha).ToList();

                }



                if (check1 == true && check2 == false)
                {
                   
                    var residentesConArchivos = entidad
                        .Where(residente => residente.Archivosenviados != null && residente.Archivosenviados.Any(x => x.NumTarea == numtarea && (x.Estatus == 1 || x.Estatus == 3)))
                        .Select(residente => new
                        {
                            Archivosenviados = residente.Archivosenviados
                                .Where(x => x.NumTarea == numtarea && (x.Estatus == 1 || x.Estatus == 3))
                                .ToList(),
                            residente.Cooasesor,
                            residente.Fecha,
                            residente.Id,
                            residente.IdCarrera,
                            residente.IdCarreraNavigation,
                            residente.IdIniciarSesion,
                            residente.IdIniciarSesionNavigation,
                            residente.NombreCompleto
                        })
                        .ToList();

                    return Ok(residentesConArchivos);

                }
                if (check2 == true && check1 == false)
                {
                    var residentesSinTareas = entidad
                   .Where(residente => residente.Archivosenviados == null || !residente.Archivosenviados.Any(archivo => archivo.NumTarea == numtarea && archivo.Estatus == 1 || archivo.NumTarea == numtarea && archivo.Estatus == 3))
                   .ToList();
                    return Ok(residentesSinTareas);
                }
                if (check1 == false && check2 == false)
                {
                    return Ok(entidad);
                }
                if (check1 == true && check2 == true)
                {
                    return Ok(entidad);
                }



                return Ok(entidad);

            }
            else
            {
                return Ok("ok");
            }



        }
        [HttpGet("nombre")]
        public IActionResult Get()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (User.IsInRole("Residente"))
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
            var id = User.Claims.FirstOrDefault(x => x.Type == "IdCarrera");

            //Role Como Coordi
            if (User.IsInRole("Admin"))
            {
                var userId = id.Value;                
                var userIdInt = int.Parse(userId);
                var trarCarrera = carreraRepository.Get(userIdInt);

                var entidad = repository
                            .GetAll()
                            .Where(x => x.IdCarrera == trarCarrera.Id)
                            .Select(x => x.Fecha.Year)
                            .Distinct();
                return Ok(entidad); 
            }
            else if (User.IsInRole("Telma"))
            {
                
                var entidad = repository
                            .GetAll()                         
                            .Select(x => x.Fecha.Year)
                            .Distinct();
                return Ok(entidad);
            }
            else
            {
                return Unauthorized("Acceso No Autorizado");

            }

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entidad = repository.Get(id);
            return Ok(entidad?.NombreCompleto);
         
        }
        [HttpPost]
        public IActionResult Post(ResidenteDTO dto)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Telma"))
            {

                var entidad = repository.Context.Inciarsesion.FirstOrDefault(x => x.Numcontrol == dto.NumControl);

                if (entidad != null)
                {
                    return BadRequest("Ya existe un alumno con el mismo numero de control ");
                }
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
            else
            {
                return Unauthorized("Acceso No Autorizado");

            }
            
        }
    
    }

}
