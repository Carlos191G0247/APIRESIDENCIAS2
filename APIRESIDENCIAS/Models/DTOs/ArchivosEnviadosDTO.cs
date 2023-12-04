namespace APIRESIDENCIAS.Models.DTOs
{
    public class ArchivosEnviadosDTO
    {
        public ArchivosEnviadosDTO()
        {
            FechaEnvio = DateTime.Now;
        }
        public int Id { get; set; }
        public int IdResidente { get; set; }

        public string NombreArchivo { get; set; } = null!;

        public DateTime FechaEnvio { get; set; }

        public int NumTarea { get; set; }

    }
}
