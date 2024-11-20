using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Semilla
{
    public int IdSemilla { get; set; }

    public string TipoSemilla { get; set; } = null!;

    public double CantidadSemilla { get; set; }

    public int IdUnidadMedida { get; set; }

    public double PrecioTotalSemilla { get; set; }

    public virtual UnidadMedidum IdUnidadMedidaNavigation { get; set; } = null!;

    public virtual ICollection<Siembra> Siembras { get; set; } = new List<Siembra>();
}
