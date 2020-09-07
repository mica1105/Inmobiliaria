using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{
    public class Pago
    {
		[Key]
		[Display(Name = "Código")]
		public int Id { get; set; }
		[Required]
		public int NroPago { get; set; }
		[Required]
		public DateTime Fecha { get; set; }
		[Required]
		public decimal Importe { get; set; }
		[Required][Display(Name = "Nro Contrato")]
		public int ContratoId { get; set; }
		[ForeignKey("ContratoId")]
		public Contrato Alquiler { get; set; }
	}
}
