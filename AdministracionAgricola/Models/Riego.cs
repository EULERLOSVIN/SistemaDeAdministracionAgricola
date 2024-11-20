using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Riego
{
    public int IdRiego { get; set; }

    public int IdLote { get; set; }

    public DateTime FechaRiego { get; set; }

    public string TipoRiego { get; set; } = null!;

    public virtual Lote IdLoteNavigation { get; set; } = null!;
}
