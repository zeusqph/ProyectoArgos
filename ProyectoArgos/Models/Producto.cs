using System;
using System.Collections.Generic;

namespace ProyectoArgos.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Franquicia { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? ImagenUrl { get; set; }

    public bool Activo { get; set; }

    public virtual DetalleComic? DetalleComic { get; set; }

    public virtual DetalleLibro? DetalleLibro { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;
}
