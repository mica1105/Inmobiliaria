using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Inmobiliaria.Models
{
    public class Contrato
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public SqlMoney PrecioMensual { get; set; }
        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }
        [Display(Name ="Inquilino")]
        public int InquilinoId { get; set; }
        [ForeignKey("InquilinoId")]
        public Inquilino inquilino { get; set; }
    }
}
