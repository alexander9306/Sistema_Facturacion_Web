using System;
using Sistema.Entidades.Almacen;
using Microsoft.EntityFrameworkCore;

namespace Sistema.Datos.Mapping.Almacen
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categoria")
                .HasKey(c => c.idcategoria);
            builder.Property(c => c.nombre)
                .HasMaxLength(50);
            builder.Property(c => c.descripcion)
                .HasMaxLength(256);
        }
    }
}