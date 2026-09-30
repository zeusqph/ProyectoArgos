using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoArgos.Models;

public partial class DbArgosLibrosContext : DbContext
{
    public DbArgosLibrosContext()
    {
    }

    public DbArgosLibrosContext(DbContextOptions<DbArgosLibrosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<DetalleComic> DetalleComics { get; set; }

    public virtual DbSet<DetalleLibro> DetalleLibros { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__Categori__A3C02A103913ACF8");

            entity.HasIndex(e => e.Nombre, "UQ__Categori__75E3EFCF7E6983F8").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DetalleComic>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__DetalleC__0988921084B91BAA");

            entity.ToTable("DetalleComic");

            entity.Property(e => e.IdProducto).ValueGeneratedNever();
            entity.Property(e => e.Editorial).HasMaxLength(50);
            entity.Property(e => e.Serie).HasMaxLength(100);

            entity.HasOne(d => d.IdProductoNavigation).WithOne(p => p.DetalleComic)
                .HasForeignKey<DetalleComic>(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetalleCo__IdPro__6A30C649");
        });

        modelBuilder.Entity<DetalleLibro>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__DetalleL__098892104C4EF42E");

            entity.ToTable("DetalleLibro");

            entity.HasIndex(e => e.Isbn, "UQ__DetalleL__447D36EA5A78AB62").IsUnique();

            entity.Property(e => e.IdProducto).ValueGeneratedNever();
            entity.Property(e => e.Autor).HasMaxLength(70);
            entity.Property(e => e.Editorial).HasMaxLength(50);
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IdProductoNavigation).WithOne(p => p.DetalleLibro)
                .HasForeignKey<DetalleLibro>(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetalleLi__IdPro__6754599E");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion).HasName("PK__Direccio__1F8E0C76E1973EBA");

            entity.ToTable("Direccion");

            entity.Property(e => e.Direccion1)
                .HasMaxLength(150)
                .HasColumnName("Direccion");
            entity.Property(e => e.Distrito).HasMaxLength(50);
            entity.Property(e => e.Referencia).HasMaxLength(150);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Direccion__IdUsu__01142BA1");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210759F4EB3");

            entity.ToTable("Producto");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Franquicia).HasMaxLength(50);
            entity.Property(e => e.ImagenUrl).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Producto__IdCate__5FB337D6");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C232AB986");

            entity.ToTable("Rol");

            entity.HasIndex(e => e.Nombre, "UQ__Rol__75E3EFCF0B6B2280").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF9741ABA19A");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A195666255E").IsUnique();

            entity.HasIndex(e => e.Dni, "UQ__Usuario__C035B8DD9568C88A").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("DNI");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdRol).HasDefaultValue(1);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__IdRol__7C4F7684");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
