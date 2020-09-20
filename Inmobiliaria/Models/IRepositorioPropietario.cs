﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioPropietario : IRepositorioConEmail<Propietario>
    {
        IList<Propietario> BuscarPorNombre(string nombre);
    }
}
