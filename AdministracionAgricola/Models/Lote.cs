using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class Lote
{
    public int IdLote { get; set; }

    public string CodigoUbicacion { get; set; } = null!;

    public double Area { get; set; }

    public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();

    public virtual ICollection<Produccion> Produccions { get; set; } = new List<Produccion>();

    public virtual ICollection<Riego> Riegos { get; set; } = new List<Riego>();

    public virtual ICollection<Siembra> Siembras { get; set; } = new List<Siembra>();
}
