using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class CategoriaPlantum
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public int? IdPersona { get; set; }

    public virtual Persona? IdPersonaNavigation { get; set; }

    public virtual ICollection<RegistroPlantum> RegistroPlanta { get; set; } = new List<RegistroPlantum>();
}
