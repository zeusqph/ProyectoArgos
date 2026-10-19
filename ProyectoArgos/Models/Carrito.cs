using System;
using System.Collections.Generic;

namespace ProyectoArgos.Models;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public int IdUsuario { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<DetalleCarrito> DetalleCarritos { get; set; } = new List<DetalleCarrito>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
