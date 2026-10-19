using System;
using System.Collections.Generic;

namespace ProyectoArgos.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int IdUsuario { get; set; }

    public int? IdDireccion { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public string? MetodoPago { get; set; }

    public string? IdTransaccionPago { get; set; }

    public DateTime FechaPedido { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Direccion? IdDireccionNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
