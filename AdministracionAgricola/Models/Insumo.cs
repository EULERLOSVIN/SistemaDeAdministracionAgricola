using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Insumo
{
    public int IdInsumo { get; set; }

    public string NombreInsumo { get; set; } = null!;

    public string TipoInsumo { get; set; } = null!;

    public double CantidadInsumo { get; set; }

    public DateTime FechaUso { get; set; }

    public int IdUnidadMedida { get; set; }

    public double PrecioUnitario { get; set; }

    public double PrecioTotal { get; set; }

    public int IdLote { get; set; }

    public virtual Lote IdLoteNavigation { get; set; } = null!;

    public virtual UnidadMedidum IdUnidadMedidaNavigation { get; set; } = null!;
}
