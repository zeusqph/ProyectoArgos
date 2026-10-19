using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoArgos.Models;
using System.Security.Claims;

namespace ProyectoArgos.Controllers
{
    [ApiController]
    [Route("api/carrito")]
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly DbArgosLibrosContext _dbcontext;

        public CarritoController(DbArgosLibrosContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCarrito()
        {
            var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idUsuarioClaim, out int idUsuario))
                return Unauthorized("No se pudo identificar al Usuario.");

            var carrito = await _dbcontext.Carritos.AsNoTracking()
                .Where(c => c.IdCarrito == idUsuario && c.Estado == "Activo")
                .Select(c => new
                {
                    c.IdCarrito,
                    c.Estado,
                    c.FechaCreacion,

                    Productos = c.DetalleCarritos.Select( d => new
                    {
                        d.IdProducto,
                        Nombre = d.IdProductoNavigation.Nombre,
                        d.Cantidad,
                        d.PrecioUnitario,
                        Subtotal = d.Cantidad * d.PrecioUnitario
                    }),

                    Total = c.DetalleCarritos.Sum( d=> 
                    (decimal?)(d.Cantidad * d.PrecioUnitario)) ?? 0
                })
                .FirstOrDefaultAsync();

            if (carrito == null)
            {
                return NotFound("No se encontró un carrito activo para el usuario.");
            }

            return Ok(carrito);

        }

    }
}
