using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;

namespace Sistema.Datos.Mapping.Almacen
{
    public class IngresoMap : IEntityTypeConfiguration<Ingreso>
    {
        public void Configure(EntityTypeBuilder<Ingreso> builder)
        {
            builder.ToTable("ingreso")
                .HasKey(i => i.idingreso);
            builder.HasOne(i => i.persona)
                .WithMany(p => p.ingresos)
                .HasForeignKey(i => i.idproveedor);
        }
    }
}
