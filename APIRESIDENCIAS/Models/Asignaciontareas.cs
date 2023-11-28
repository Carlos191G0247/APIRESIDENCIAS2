using System;
using System.Collections.Generic;

namespace APIRESIDENCIAS.Models;

public partial class Asignaciontareas
{
    public int Id { get; set; }

    public int Idcoordinador { get; set; }

    public string NombreTarea { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string Intruccion { get; set; } = null!;

    public int NumTarea { get; set; }

    public virtual Coordinadores IdcoordinadorNavigation { get; set; } = null!;
}
