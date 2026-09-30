using System;
using System.Collections.Generic;

namespace ProyectoArgos.Models;

public partial class DetalleComic
{
    public int IdProducto { get; set; }

    public string? Serie { get; set; }

    public int? NumeroTomo { get; set; }

    public string? Editorial { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
