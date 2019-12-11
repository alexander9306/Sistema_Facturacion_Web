using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Ventas;
using Sistema.Web.Models.Ventas.Venta;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public VentasController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Ventas/Listar
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .OrderByDescending(v => v.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,
                telefono = v.persona.telefono,
                email = v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado
            });
        }

        // GET: api/Ventas/VentasMes12
        [Authorize(Roles = "Almacenero,Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ConsultaViewModel>> VentasMes12()
        {
            var consulta = await _context.Ventas
                .GroupBy(v=>v.fecha_hora.Month)
                .Select(x=>new { Etiqueta=x.Key, Valor=x.Sum(v=>v.total)})
                .OrderByDescending(x => x.Etiqueta)
                .Take(12)
                .ToListAsync();

            return consulta.Select(v => new ConsultaViewModel
            {
                etiqueta=v.Etiqueta.ToString(),
                valor=v.Valor
            });
        }

        // GET: api/Ventas/ConsultaFechas/FechaInicio/FechaFin
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{FechaInicio}/{FechaFin}")]
        public async Task<IEnumerable<VentaViewModel>> ConsultaFechas([FromRoute] DateTime fechaInicio, DateTime fechaFin)
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .Where(i=>i.fecha_hora>= fechaInicio)
                .Where(i=>i.fecha_hora<= fechaFin)
                .OrderByDescending(v => v.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento=v.persona.num_documento,
                direccion=v.persona.direccion,
                telefono=v.persona.telefono,
                email=v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado
            });
        }

        // GET: api/Ventas/ListarFiltro/texto
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {
            var venta = await _context.Ventas 
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .Where(v => v.num_comprobante.Contains(texto))
                .OrderByDescending(v => v.idventa)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,
                telefono = v.persona.telefono,
                email = v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado
            });
        }

        // GET: api/Ventas/ListarDetalles
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{idventa}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idventa)
        {
            var detalle = await _context.DetallesVentas
                .Include(a => a.articulo)
                .Where(d => d.idventa == idventa)
                .ToListAsync().ConfigureAwait(false);

            return detalle.Select(d => new DetalleViewModel
            {
                idarticulo = d.idarticulo,
                articulo = d.articulo.nombre,
                cantidad = d.cantidad,
                precio = d.precio,
                descuento = d.descuento
            });
        }


        // POST: api/Ventas/Crear
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest((ModelState));
            }

            var fechaHora = DateTime.Now;

            Venta venta = new Venta
            {
                idcliente = model.idcliente,
                idusuario = model.idusuario,
                tipo_comprobante = model.tipo_comprobante,
                serie_comprobante = model.serie_comprobante,
                num_comprobante = model.num_comprobante,
                fecha_hora = fechaHora,
                impuesto = model.impuesto,
                total = model.total,
                estado = "Aceptado"
            };


            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                var id = venta.idventa;
                foreach (var det in model.detalles)
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        idventa = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio,
                        descuento = det.descuento
                    };
                    _context.DetallesVentas.Add(detalle);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

            return Ok();
        }


        // PUT: api/Ventas/Anular
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(v => v.idventa == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.estado = "Anulado";


            try
            {
                await _context.SaveChangesAsync();
                var detalle = await _context.DetallesVentas
                    .Include(a => a.articulo)
                    .Where(d => d.idventa == id).ToListAsync();
                foreach (var det in detalle)
                {
                    var articulo =
                        await _context.Articulos
                            .FirstOrDefaultAsync(a => a.idarticulo == det.articulo.idarticulo);
                    articulo.stock = det.articulo.stock + det.cantidad;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                //Guardar Excepcion
                return BadRequest();
            }
            return Ok();
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.idventa == id);
        }
    }
}
