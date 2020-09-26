using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
        IList<Contrato> BuscarPorInmueble(int IdInmueble);
        IList<Contrato> ContradosVigentes(DateTime fecha);
    }
}
