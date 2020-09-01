using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Inmobiliaria_.Net_Core.Models;
using System.Data.SqlTypes;

namespace Inmobiliaria.Models
{
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Tipo { get; set; }
        public int Ambientes { get; set; }
        [Required]
        public string Uso { get; set; }
        public SqlMoney Precio { get; set; }
        public int Estado { get; set; }
        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }
    }
}

