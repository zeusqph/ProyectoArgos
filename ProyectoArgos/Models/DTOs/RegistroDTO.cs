
using System.ComponentModel.DataAnnotations;

namespace ProyectoArgos.Models.DTOs
{
    public class RegistroDTO
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; } = null!;

        [Required, StringLength(50)]
        public string Apellido { get; set; } = null!;

        [Required, EmailAddress, StringLength(100)]
        public string Correo { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = null!;

        [Required, RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos.")]
        public string Dni { get; set; } = null!;

        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos.")]
        public string? Telefono { get; set; }

    }
}
