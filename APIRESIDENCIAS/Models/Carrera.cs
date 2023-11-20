using System;
using System.Collections.Generic;

namespace APIRESIDENCIAS.Models;

public partial class Carrera
{
    public int Id { get; set; }

    public string NombreCarrera { get; set; } = null!;

    public virtual ICollection<Coordinadores> Coordinadores { get; set; } = new List<Coordinadores>();

    public virtual ICollection<Residentes> Residentes { get; set; } = new List<Residentes>();
}
