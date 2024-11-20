using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Persona
{
    public int IdPersona { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int Dni { get; set; }

    public string? PkAspNetUsers { get; set; }

    public virtual ICollection<CategoriaPlantum> CategoriaPlanta { get; set; } = new List<CategoriaPlantum>();

    public virtual AspNetUser? PkAspNetUsersNavigation { get; set; }
}
