using System.ComponentModel.DataAnnotations;



namespace ProyectoArgos.Models.DTOs
{
    public class ProductoCrearDTO
    {
        [Required]
        public int IdCategoria { get; set; }

        [Required,StringLength(100)]
        public string Nombre { get; set; } = null!;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Range(0, 10000)]
        public decimal Precio { get; set; }
        [Range(0,10000)]
        public int Stock { get; set; }

        [StringLength(300)]
        public string? ImagenUrl { get; set; }

        [StringLength(50)]
        public string? Franquicia { get; set; }


        [StringLength(13)]
        public string? Isbn { get; set; }

        [StringLength(50)]
        public string? Editorial { get; set; }

        [StringLength(70)]
        public string? Autor { get; set; }


        public class ProductoDTO
        {
            public int IdProducto { get; set; }
            public int IdCategoria { get; set; }
            public string Categoria { get; set; } = null!;
            public string Nombre { get; set; } = null!;
            public string? Descripcion { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public string? ImagenUrl { get; set; }
            public string? Franquicia { get; set; }
            public string? Isbn { get; set; }
            public string? Editorial { get; set; }
            public string? Autor { get; set; }
        }

    }
}
