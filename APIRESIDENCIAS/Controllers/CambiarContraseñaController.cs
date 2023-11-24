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
    public class CambiarContraseñaController : ControllerBase
    {
        private readonly IniciarSesionRepository repository;

        public CambiarContraseñaController(IniciarSesionRepository repository)
        {
            this.repository = repository;
        }

    
        [HttpPost("login")]
        public IActionResult PostLogin(CambiarContraseñaDTO dto)
        {
            try
            {
                var usuario = repository.GetAll().FirstOrDefault(x => x.Numcontrol == dto.Numcontrol);
                if (usuario == null)
                {
                    return NotFound("Número de control incorrecto");
                }
                const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                char[] contraseña = new char[10];
                for (int i = 0; i < 10; i++)
                {
                    contraseña[i] = caracteresPermitidos[random.Next(caracteresPermitidos.Length)];

                }
                EnviarCorreoElectronico("enriquesanmiguel32@gmail.com", "Nueva Contraseña", $"Tu nueva contraseña es: {new string(contraseña)}");

                return Ok();

            }
            catch (Exception)
            {

                return NotFound("Error al enviar el correo electrónico");
            }

        
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
