using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioConEmail<T> : IRepositorio<T>
    {
        T ObtenerPorEmail(string email);
    }
}
