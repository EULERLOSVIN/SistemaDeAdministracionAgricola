using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class RegistroPlantum
{
    public int IdRegistroPlanta { get; set; }

    public string NombrePlanta { get; set; } = null!;

    public int IdCategoria { get; set; }

    public virtual CategoriaPlantum IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<Siembra> Siembras { get; set; } = new List<Siembra>();
}
