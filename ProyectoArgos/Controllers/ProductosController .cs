using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoArgos.Models;
using ProyectoArgos.Models.DTOs;
using System.Linq.Expressions;
using static ProyectoArgos.Models.DTOs.ProductoCrearDTO;


namespace ProyectoArgos.Controllers
{
    [ApiController] 
    [Route("api/productos")]
    public class ProductosController : Controller
    {

        private readonly DbArgosLibrosContext _dbcontext;

        public ProductosController(DbArgosLibrosContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        private static readonly Expression<Func<Producto, ProductoDTO>> ADto = p => new ProductoDTO
        {
            IdProducto = p.IdProducto,
            IdCategoria = p.IdCategoria,
            Categoria = p.IdCategoriaNavigation.Nombre,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Stock = p.Stock,
            ImagenUrl = p.ImagenUrl,
            Franquicia = p.Franquicia,
            Isbn = p.DetalleLibro != null ? p.DetalleLibro.Isbn : null,
            Editorial = p.DetalleLibro != null ? p.DetalleLibro.Editorial : null,
            Autor = p.DetalleLibro != null ? p.DetalleLibro.Autor : null
        };


        private static string? Limpiar(string? s) =>
            string.IsNullOrWhiteSpace(s) ? null : s.Trim();


        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] int? categoria,
            [FromQuery] string? buscar,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamano = 20)
        {
            pagina = Math.Max(pagina, 1);
            tamano = Math.Clamp(tamano, 1, 50);

            var query = _dbcontext.Productos.AsNoTracking().Where(p => p.Activo);

            if (categoria.HasValue)
                query = query.Where(p => p.IdCategoria == categoria.Value);

            if (!string.IsNullOrWhiteSpace(buscar))
                query = query.Where(p => p.Nombre.Contains(buscar.Trim()));

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Nombre)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .Select(ADto)
                .ToListAsync();

            return Ok(new { total, pagina, tamano, items });
        }

        // GET api/productos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _dbcontext.Productos.AsNoTracking()
                .Where(p => p.IdProducto == id && p.Activo)
                .Select(ADto)
                .FirstOrDefaultAsync();
            return producto == null ? NotFound() : Ok(producto);
        }


        // GET api/productos/categorias
        [HttpGet("categorias")]
        public async Task<IActionResult> Categorias()
        {
            var categoria = await _dbcontext.Categoria.AsNoTracking()
                .OrderBy(c => c.Nombre)
                .Select(c => new { c.IdCategoria, c.Nombre })
                .ToListAsync();

            return Ok(categoria); 
        }


        // ---------- SOLO ADMIN ----------
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Crear(ProductoCrearDTO dto)
        {
            if (!await _dbcontext.Categoria.AnyAsync(c => c.IdCategoria == dto.IdCategoria))
                return BadRequest("La categoria no existe");


            var isbn = Limpiar(dto.Isbn);
            if (isbn != null && await _dbcontext.DetalleLibros.AnyAsync(d => d.Isbn == isbn))
                return Conflict("Ya existe ese producto con ese ISBN");

            var producto = new Producto
            {
                IdCategoria = dto.IdCategoria,
                Nombre = dto.Nombre.Trim(),
                Descripcion = Limpiar(dto.Descripcion),
                Precio = dto.Precio,
                Stock = dto.Stock,
                ImagenUrl = Limpiar(dto.ImagenUrl),
                Franquicia = Limpiar(dto.Franquicia),
                Activo = true
            };

            var editorial = Limpiar(dto.Editorial);
            var autor = Limpiar(dto.Autor);
            if (isbn != null || editorial != null || autor != null)
            {
                producto.DetalleLibro = new DetalleLibro
                {
                    Isbn = isbn,
                    Editorial = editorial,
                    Autor = autor
                };
            }

            _dbcontext.Productos.Add(producto);
            await _dbcontext.SaveChangesAsync();

            var resultado = await _dbcontext.Productos.AsNoTracking()
                .Where(p => p.IdProducto == producto.IdProducto)
                .Select(ADto)
                .FirstAsync();

            return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.IdProducto }, resultado);


        }


        [HttpPut("{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Editar(int id,ProductoCrearDTO dto)
        {
            var producto = await _dbcontext.Productos
                .Include(p => p.DetalleLibro)
                .FirstOrDefaultAsync(p => p.IdProducto == id && p.Activo);

            if (producto == null) return NotFound();
            if (!await _dbcontext.Categoria.AnyAsync(c => c.IdCategoria == dto.IdCategoria))
                return BadRequest("La categoria no existe");

            var isbn = Limpiar(dto.Isbn);
            if (isbn != null && await _dbcontext.DetalleLibros.AnyAsync(d => d.Isbn == isbn && d.IdProducto != id))
                return Conflict("Ya existe ese producto con ese ISBN");

            producto.IdCategoria = dto.IdCategoria;
            producto.Nombre = dto.Nombre.Trim();
            producto.Descripcion = Limpiar(dto.Descripcion);
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.ImagenUrl = Limpiar(dto.ImagenUrl);
            producto.Franquicia = Limpiar(dto.Franquicia);


            var editorial = Limpiar(dto.Editorial);
            var autor = Limpiar(dto.Autor);
            var tieneDetalle = isbn != null || editorial != null || autor != null;

            if (tieneDetalle)
            {
                producto.DetalleLibro ??= new DetalleLibro();
                producto.DetalleLibro.Isbn = isbn;
                producto.DetalleLibro.Editorial = editorial;
                producto.DetalleLibro.Autor = autor;
            }

            else if (producto.DetalleLibro != null)
            {
                _dbcontext.DetalleLibros.Remove(producto.DetalleLibro);
            }

            await _dbcontext.SaveChangesAsync();

            var resultado = _dbcontext.Productos.AsNoTracking()
                .Where(p => p.IdProducto == id)
                .Select(ADto)
                .FirstAsync();

            return Ok(resultado);

        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _dbcontext.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == id && p.Activo);

            if (producto == null) return NotFound();

            producto.Activo = false;
            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
