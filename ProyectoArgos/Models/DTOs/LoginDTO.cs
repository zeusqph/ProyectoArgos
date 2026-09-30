using System.ComponentModel.DataAnnotations;

namespace ProyectoArgos.Models.DTOs
{
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string Correo { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
