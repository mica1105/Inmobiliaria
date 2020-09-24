using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Inmobiliaria.Models
{
    public enum enEstados
    {
        Disponible = 1,
        En_Refaccion = 2,
        Suspendido = 3,
    }
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
        [Required]
        public decimal Precio { get; set; }  
        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Duenio { get; set; }
        public int Estado { get; set; }
        [Display(Name = "Estado")]
        public string EstadoNombre => Estado > 0 ? ((enEstados)Estado).ToString().Replace('_',' ') : "";
        public static IDictionary<int, string> ObtenerEstados()
        {
            SortedDictionary<int, string> estados = new SortedDictionary<int, string>();
            Type tipoEnumEstados = typeof(enEstados);
            foreach (var valor in Enum.GetValues(tipoEnumEstados))
            {
                estados.Add((int)valor, Enum.GetName(tipoEnumEstados, valor).Replace('_', ' '));
            }
            return estados;
        }
        
    }
}

