using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Siembra
{
    public int IdSiembra { get; set; }

    public DateTime FechaSiembra { get; set; }

    public int IdLote { get; set; }

    public string TipoSiembra { get; set; } = null!;

    public int IdRegistroPlanta { get; set; }

    public int IdClima { get; set; }

    public int IdSemilla { get; set; }

    public virtual ICollection<ControlPerdidum> ControlPerdida { get; set; } = new List<ControlPerdidum>();

    public virtual Clima IdClimaNavigation { get; set; } = null!;

    public virtual Lote IdLoteNavigation { get; set; } = null!;

    public virtual RegistroPlantum IdRegistroPlantaNavigation { get; set; } = null!;

    public virtual Semilla IdSemillaNavigation { get; set; } = null!;
}
