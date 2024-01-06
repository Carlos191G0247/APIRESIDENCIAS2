namespace APIRESIDENCIAS.Models.DTOs
{
    public class ResidenteDTO
    {

        public string NombreCompleto { get; set; } = null!;
        public string Cooasesor { get; set; } = null!;
        public DateTime Fecha { get; set; }
        //Tabla Correo
        public int Carrera { get; set; }


        //Tabla IniciarSesion
        public string Contrasena { get; set; } = null!;
        public string NumControl { get; set; } = null!;
    }
}
