namespace APIRESIDENCIAS.Models.DTOs
{
    public class EditarDTO
    {

        public int Idcoordinador { get; set; }

        public string NombreTarea { get; set; } = null!;

        public DateTime Fecha { get; set; }

        public string Intruccion { get; set; } = null!;

        public int NumTarea { get; set; }

    }
}
