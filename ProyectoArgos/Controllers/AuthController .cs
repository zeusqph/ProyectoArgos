using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoArgos.Custom;
using ProyectoArgos.Mappers;
using ProyectoArgos.Models;
using ProyectoArgos.Models.DTOs;
using ProyectoArgos.Services;
using System.Security.Claims;

namespace ProyectoArgos.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly DbArgosLibrosContext _dbcontext;
        private readonly PasswordService _password;
        private readonly TokenService _token;
        private readonly IConfiguration _config;

        public AuthController(
            DbArgosLibrosContext context,
            PasswordService password,
            TokenService token,
            IConfiguration config)
        {
            _dbcontext = context;
            _password = password;
            _token = token;
            _config = config;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroDTO dto)
        {
            var correo = dto.Correo.Trim().ToLower();
            if (await _dbcontext.Usuarios.AnyAsync(u => u.Correo == correo))
                return Conflict("El correo ya esta registrado");

            // Vacío -> null, para no chocar con el índice único filtrado
            var dni = string.IsNullOrWhiteSpace(dto.Dni) ? null : dto.Dni.Trim();

            if (dni != null && await _dbcontext.Usuarios.AnyAsync(u => u.Dni == dni))
                return Conflict("El DNI ya esta registrado");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Correo = correo,
                Telefono = dto.Telefono,
                Dni = dni,
                PasswordHash = _password.Hashear(dto.Password),
                IdRol = 1,               // Cliente, lo fija el servidor
                Activo = true,
                FechaRegistro = DateTime.Now
            };

            _dbcontext.Usuarios.Add(usuario);
            await _dbcontext.SaveChangesAsync();

            // Recargar con el rol para poder mapear al DTO
            await _dbcontext.Entry(usuario).Reference(u => u.IdRolNavigation).LoadAsync();

            return Ok(usuario.ToDto());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var correo = dto.Correo.Trim().ToLower();

            var usuario = await _dbcontext.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Activo);

            if (usuario == null || !_password.Verificar(dto.Password, usuario.PasswordHash))
                return Unauthorized("Correo o contraseña incorrecta");

            var minutos = int.Parse(_config["Jwt:ExpirationInMinutes"] ?? "60");

            return Ok(new AuthResponseDto
            {
                Token = _token.generarTokenJWT(usuario),
                Expira = DateTime.UtcNow.AddMinutes(minutos),
                Usuario = usuario.ToDto()
            });
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var usuario = await _dbcontext.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario == id && u.Activo);

            return usuario == null ? NotFound() : Ok(usuario.ToDto());
        }
    }
}