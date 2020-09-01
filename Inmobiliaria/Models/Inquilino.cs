using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
	public class Inquilino
	{
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required]
		public string Dni { get; set; }
		[Required][Display(Name = "Lugar de Trabajo")]
		public string LugarTrabajo { get; set; }
		public string Telefono { get; set; }
		[Required, EmailAddress]
		public string Email { get; set; }
		[Required]
		[Display(Name = "Nombre")]
		public string NombreGarante { get; set; }
		[Required]
		[Display(Name = "Dni")]
		public string DniGarante { get; set; }
		[Display(Name = "Teléfono")]
		public string TelefonoGarante { get; set; }
	}
}
