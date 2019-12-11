﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Sistema.Entidades.Usuarios;

namespace Sistema.Entidades.Ventas
{
    public class Venta
    {
        public int idventa { get; set; }
        [Required]
        public int idcliente { get; set; }
        [Required]
        public int idusuario { get; set; }
        [Required]
        public string tipo_comprobante { get; set; }
        public string serie_comprobante { get; set; }
        [Required]
        public string num_comprobante { get; set; }
        [Required]
        public DateTime fecha_hora { get; set; }
        [Required]
        public decimal impuesto { get; set; }
        [Required]
        public decimal total { get; set; }
        [Required]
        public string estado { get; set; }

        public ICollection<DetalleVenta> detalles { get; set; }
        public Usuario usuario { get; set; }
        public Persona persona { get; set; }
    }
}
