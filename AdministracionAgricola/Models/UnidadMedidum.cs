using System;
using System.Collections.Generic;

namespace AdministracionAgricola.Models;

public partial class UnidadMedidum
{
    public int IdUnidadMedida { get; set; }

    public string NombreMedida { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Clima> Climas { get; set; } = new List<Clima>();

    public virtual ICollection<ControlPerdidum> ControlPerdida { get; set; } = new List<ControlPerdidum>();

    public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();

    public virtual ICollection<Produccion> Produccions { get; set; } = new List<Produccion>();

    public virtual ICollection<Semilla> Semillas { get; set; } = new List<Semilla>();
}
