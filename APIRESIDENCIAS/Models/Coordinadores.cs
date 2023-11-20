using System;
using System.Collections.Generic;

namespace APIRESIDENCIAS.Models;

public partial class Coordinadores
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public int IdIniciarSesion { get; set; }

    public int IdCarrera { get; set; }

    public virtual ICollection<Asignaciontareas> Asignaciontareas { get; set; } = new List<Asignaciontareas>();

    public virtual Carrera IdCarreraNavigation { get; set; } = null!;

    public virtual Inciarsesion IdIniciarSesionNavigation { get; set; } = null!;
}
