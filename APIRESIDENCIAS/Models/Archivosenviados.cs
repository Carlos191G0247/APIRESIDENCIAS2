using System;
using System.Collections.Generic;

namespace APIRESIDENCIAS.Models;

public partial class Archivosenviados
{
    public int Id { get; set; }

    public int IdResidente { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public int NumTarea { get; set; }

    public virtual Residentes IdResidenteNavigation { get; set; } = null!;
}
