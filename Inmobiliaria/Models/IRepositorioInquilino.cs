﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        IList<Inquilino> BuscarPorNombre(string nombre);
    }
}
