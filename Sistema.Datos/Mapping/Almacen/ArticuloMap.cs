using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;

namespace Sistema.Datos.Mapping.Almacen
{
    class ArticuloMap : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder)
        {
            builder.ToTable("articulo")
                .HasKey(a => a.idarticulo);
        }
    }
}
