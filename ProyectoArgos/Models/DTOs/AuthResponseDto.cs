
namespace ProyectoArgos.Models.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expira { get; set; }
        public UsuarioDTO Usuario { get; set; } = null!;
    }
}
