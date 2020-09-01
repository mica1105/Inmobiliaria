using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        RepositorioInmueble repositorio;
        public InmuebleController(IConfiguration configuration)
        {
            repositorio = new RepositorioInmueble(configuration);
        }

        // GET: InmuebleController
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var i = repositorio.ObtenerPorId(id);
            return View();
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                repositorio.Alta(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var i = repositorio.ObtenerPorId(id);
            return View(i);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                repositorio.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repositorio.ObtenerPorId(id);
            return View(i);
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
