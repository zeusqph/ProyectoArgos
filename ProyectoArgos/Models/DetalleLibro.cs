using System;
using System.Collections.Generic;

namespace ProyectoArgos.Models;

public partial class DetalleLibro
{
    public int IdProducto { get; set; }

    public string? Isbn { get; set; }

    public string? Editorial { get; set; }

    public string? Autor { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
