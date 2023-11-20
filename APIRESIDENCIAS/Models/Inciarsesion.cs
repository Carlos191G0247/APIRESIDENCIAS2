using System;
using System.Collections.Generic;

namespace APIRESIDENCIAS.Models;

public partial class Inciarsesion
{
    public int Id { get; set; }

    public string Contrasena { get; set; } = null!;

    public string Numcontrol { get; set; } = null!;

    public virtual ICollection<Coordinadores> Coordinadores { get; set; } = new List<Coordinadores>();

    public virtual ICollection<Residentes> Residentes { get; set; } = new List<Residentes>();
}
