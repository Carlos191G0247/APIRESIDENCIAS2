using APIRESIDENCIAS.Models.DTOs;
using APIRESIDENCIAS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace APIRESIDENCIAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CambiarContrasenaController : ControllerBase
    {
        private readonly IniciarSesionRepository repository;
        

        public CambiarContrasenaController(IniciarSesionRepository repository)
        {
            this.repository = repository;
        }

    
        [HttpPost("login")]
        public IActionResult PostLogin(NumeroControlDTO dto)
        {
            
                var usuario = repository.GetAll().FirstOrDefault(x => x.Numcontrol == dto.Numcontrol);
                if (usuario == null)
                {
                    return NotFound("Número de control incorrecto");
                }
          
                return Ok(usuario.Id);
        }

        [HttpPut]
        public IActionResult Put(CambiarContraseñaDTO dto)
        {

            if (ModelState.IsValid)
            {
                var buscar = repository.Get(dto.Id);

                if (buscar != null)
                {
                    const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var random = new Random();
                    char[] contraseña = new char[10];
                    for (int i = 0; i < 10; i++)
                    {
                        contraseña[i] = caracteresPermitidos[random.Next(caracteresPermitidos.Length)];

                    }
                    buscar.Contrasena = new string(contraseña);
                    EnviarCorreoElectronico(buscar.Numcontrol + "@rcarbonifera.tecnm.mx", "Nueva Contraseña", $"Tu nueva contraseña es: {new string(contraseña)}");

                    repository.Update(buscar);

                }
            }
            return Ok();

        }

        private void EnviarCorreoElectronico(string destinatario, string asunto, string cuerpo)
        {
            var smtpCliente = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("residenciaswebitesrc@outlook.com", "Tecolote1991"),
                EnableSsl = true,
            };
            var mensaje = new MailMessage("residenciaswebitesrc@outlook.com", destinatario, asunto, cuerpo);
            smtpCliente.Send(mensaje);
        }
    }
}
