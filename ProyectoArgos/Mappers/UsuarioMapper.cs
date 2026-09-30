
using ProyectoArgos.Models;
using ProyectoArgos.Models.DTOs;

namespace ProyectoArgos.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioDTO ToDto(this Usuario u) => new()
        {
            IdUsuario = u.IdUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Correo = u.Correo,
            Telefono = u.Telefono,
            Rol = u.IdRolNavigation.Nombre   // requiere Include(u => u.IdRolNavigation)
        };
    }
}