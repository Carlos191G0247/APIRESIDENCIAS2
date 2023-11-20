using System;
using System.Collections.Generic;

namespace APIRESIDENCIAS.Models;

public partial class Solicitudesalumnos
{
    public int Idsolicitudesalumnos { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string NumeroDeControl { get; set; } = null!;

    public int Semestre { get; set; }

    public string Grupo { get; set; } = null!;

    public string Cooasesor { get; set; } = null!;

    public string Carrera { get; set; } = null!;

    public string? Aprobado { get; set; }
}
