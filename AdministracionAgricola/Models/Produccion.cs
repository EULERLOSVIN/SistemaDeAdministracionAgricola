using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Produccion
{
    public int IdProduccion { get; set; }

    public int IdLote { get; set; }

    public string TipoProducto { get; set; } = null!;

    public double CantidadProduccion { get; set; }

    public int IdUnidadMedida { get; set; }

    public DateTime FechaCosecha { get; set; }

    public virtual Lote IdLoteNavigation { get; set; } = null!;

    public virtual UnidadMedidum IdUnidadMedidaNavigation { get; set; } = null!;
}
