using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class ControlPerdidum
{
    public int IdControlPerdida { get; set; }

    public int IdSiembra { get; set; }

    public DateTime FechaPerdida { get; set; }

    public string CausaPlaga { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int CantidadPerdida { get; set; }

    public int IdUnidadMedida { get; set; }

    public virtual Siembra IdSiembraNavigation { get; set; } = null!;

    public virtual UnidadMedidum IdUnidadMedidaNavigation { get; set; } = null!;
}
