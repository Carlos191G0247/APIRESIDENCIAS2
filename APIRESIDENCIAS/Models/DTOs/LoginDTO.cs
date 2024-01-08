namespace APIRESIDENCIAS.Models.DTOs
{
    public class LoginDTO
    {
        public string Contrasena { get; set; } = null!;
        public string Numcontrol { get; set; } = null!;

        public string Rol { get; set; } = null!;
    }
}
