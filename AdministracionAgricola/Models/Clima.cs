using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Clima
{
    public int IdClima { get; set; }

    public double Temperatura { get; set; }

    public int IdUnidadMedida { get; set; }

    public string EstacionAnio { get; set; } = null!;

    public string? DescripcionCondiciones { get; set; }

    public virtual UnidadMedidum IdUnidadMedidaNavigation { get; set; } = null!;

    public virtual ICollection<Siembra> Siembras { get; set; } = new List<Siembra>();
}
