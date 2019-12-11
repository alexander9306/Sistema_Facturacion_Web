using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Sistema.Entidades.Usuarios;

namespace Sistema.Datos.Mapping.Usuarios
{
    public class RolMap : IEntityTypeConfiguration<Rol>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("rol")
                .HasKey(r => r.idrol);
        }
    }
}
