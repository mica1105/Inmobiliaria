using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioPago 
    {
        int Alta(Pago p);
        int Baja(int id);
        int Modificacion(Pago p);
        IList<Pago> ObtenerTodos(int id);
        Pago ObtenerPorId(int id);
    }
}
