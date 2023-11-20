using System;
using System.Collections.Generic;

namespace APIRESIDENCIAS.Models;

public partial class Residentes
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string? Cooasesor { get; set; }

    public int IdCarrera { get; set; }

    public int IdIniciarSesion { get; set; }

    public DateTime Fecha { get; set; }

    public virtual ICollection<Archivosenviados> Archivosenviados { get; set; } = new List<Archivosenviados>();

    public virtual Carrera IdCarreraNavigation { get; set; } = null!;

    public virtual Inciarsesion IdIniciarSesionNavigation { get; set; } = null!;
}
