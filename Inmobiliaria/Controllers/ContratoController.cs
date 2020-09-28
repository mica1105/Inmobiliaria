using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private IRepositorioContrato repositorio;
        private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorio<Inquilino> repoInquilino;

        public ContratoController(IRepositorioContrato repositorio, IRepositorioInmueble repositorioInmueble,IRepositorio<Inquilino> repositorioInquilino)
        {
            this.repositorio = repositorio;
            repoInmueble = repositorioInmueble;
            repoInquilino = repositorioInquilino;
        }

        // GET: ContratoController
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            return View(lista);
        }

        public ActionResult PorInmueble(int id)
        {
            try
            {
                var lista = repositorio.BuscarPorInmueble(id);
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult Vigentes()
        {
            try
            {
                var hoy = DateTime.Now;
                var lista = repositorio.ContradosVigentes(hoy);
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: ContratoController/Create
        [Authorize]
        public ActionResult Create(string desde, string hasta)
        {
            DateTime inicio = DateTime.Parse(desde);
            DateTime fin = DateTime.Parse(hasta);
            ViewBag.Desde = inicio;
            ViewBag.Hasta = fin;
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            ViewBag.Inmuebles = repoInmueble.BuscarPorFechas(inicio,fin);
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(contrato);
                    TempData["Id"] = contrato.Id;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
                    ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
                    return View(contrato);
                }
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ContratoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                repositorio.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}
