using Microsoft.EntityFrameworkCore;
using Sistema.Datos.Mapping.Almacen;
using Sistema.Datos.Mapping.Usuarios;
using Sistema.Datos.Mapping.Ventas;
using Sistema.Entidades.Almacen;
using Sistema.Entidades.Usuarios;
using Sistema.Entidades.Ventas;

namespace Sistema.Datos
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<DetalleIngreso> DetallesIngresos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVentas { get; set; }

        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new IngresoMap());
            modelBuilder.ApplyConfiguration(new DetalleIngresoMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new DetalleVentaMap());
        }

    }
}
