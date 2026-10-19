using System;
using System.Collections.Generic;

namespace ProyectoArgos.Models;

public partial class DetalleCarrito
{
    public int IdDetalleCarrito { get; set; }

    public int IdCarrito { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public DateTime FechaAgregado { get; set; }

    public virtual Carrito IdCarritoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
