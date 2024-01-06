using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIRESIDENCIAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IniciarSesionController : ControllerBase
    {
        private readonly IniciarSesionRepository repository;
        public IniciarSesionController(IniciarSesionRepository repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            //var login = repository.GetAll().SingleOrDefault(x => x.Numcontrol == loginDTO.Numcontrol && x.Contrasena == loginDTO.Contrasena);
            var login = repository.Context.Inciarsesion.Include(x=>x.Coordinadores).SingleOrDefault(x => x.Numcontrol == loginDTO.Numcontrol && x.Contrasena == loginDTO.Contrasena);
            try
            {
                //checar si existe en la bd
                if (login != null)
                {
                    List<Claim> cliams = new()
                {
                    new Claim("Id",login.Id.ToString()),
                    new Claim(ClaimTypes.Name,loginDTO.Numcontrol),
                    new Claim("IdCarrera",login.Coordinadores.FirstOrDefault()?.IdCarrera.ToString()??""),
                    new Claim(ClaimTypes.Role,"Admin"),
                 };

                    SecurityTokenDescriptor tokenDescriptor = new()
                    {
                        Issuer = "https://localhost:7136",
                        Audience = "localhost:7136",
                        IssuedAt = DateTime.UtcNow,
                        Expires = DateTime.UtcNow.AddMinutes(30),
                        SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("residencias9.1G1234567890"))
                                , SecurityAlgorithms.HmacSha256),
                        Subject = new ClaimsIdentity(cliams, JwtBearerDefaults.AuthenticationScheme)
                    };

                    JwtSecurityTokenHandler handler = new();
                    var token = handler.CreateToken(tokenDescriptor);

                    return Ok(handler.WriteToken(token));
                }
                else
                    return Unauthorized("Nombre de usuario o contraseña incorrectas.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
    }
}
