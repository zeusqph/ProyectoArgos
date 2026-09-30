using System;
using System.Collections.Generic;

namespace ProyectoArgos.Models;

public partial class Direccion
{
    public int IdDireccion { get; set; }

    public int IdUsuario { get; set; }

    public string Direccion1 { get; set; } = null!;

    public string? Distrito { get; set; }

    public string? Referencia { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
