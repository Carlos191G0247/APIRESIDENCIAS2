﻿using APIRESIDENCIAS.Models;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIRESIDENCIAS.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoordinadoresController : ControllerBase
    {
        private readonly  CoordinadoresRepository repository;
        private readonly CarreraRepository carreraRepository;
        public CoordinadoresController(CoordinadoresRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("nom")]
        public async Task<IActionResult> Get()
        {
            

            //Verificar si se usa


            //List<Coordinadores> xd = new();

            //xd = await repository.Context.Coordinadores.Include(x => x.IdCarreraNavigation).ToListAsync();
            var entidades = repository.Context.Coordinadores.Include(x => x.IdCarreraNavigation).ToList();

            return Ok(entidades);

        }


        //Rol para admim
        [HttpGet("CordiNom")]
        public IActionResult GetNombre()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (User.IsInRole("Admin") || User.IsInRole("Telma"))
            {
                var userId = id.Value;
                var userIdInt = int.Parse(userId);
                var entidad = repository.GetAll().SingleOrDefault(x => x.IdIniciarSesion == userIdInt);
                return Ok(entidad.NombreCompleto);
            }
            else
            {
                return Unauthorized("Acceso No Autorizado");
            }


        }

    }
}



//var Carrera = carreraRepository.GetAll().Where(x=>x.NombreCarrera )
