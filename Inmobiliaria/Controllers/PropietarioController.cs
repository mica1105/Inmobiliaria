using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        RepositorioPropietario repositorio;
        public PropietarioController()
        {
             repositorio = new RepositorioPropietario();
            
        }

        // GET: Propietario
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }


        // GET: Propietario/Buscar/5
        [Route("[controller]/Buscar/{q}", Name = "Buscar")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = repositorio.BuscarPorNombre(q);
                return Json(new { Datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        // GET: Propietario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        { 

            try
            {
                    repositorio.Alta(propietario);
                    return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = null;
            try
            {
                p = repositorio.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Email = collection["Email"];
                p.Telefono = collection["Telefono"];
                repositorio.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
        // GET: Propietario/Delete
        public ActionResult Delete(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        // POST: Propietario/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario entidad)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}