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
        private readonly IRepositorioInquilino repoInquilino;

        public ContratoController(IRepositorioContrato repositorio, IRepositorioInmueble repositorioInmueble,IRepositorioInquilino repositorioInquilino)
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
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(lista);
        }

        public ActionResult PorInmueble(int id)
        {
            try
            {
                var lista = repositorio.BuscarPorInmueble(id);
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }
        public ActionResult Vigentes()
        {
            try
            {
                var hoy = DateTime.Now;
                var lista = repositorio.ContradosVigentes(hoy);
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (TempData.ContainsKey("Error"))
                    ViewBag.Error = TempData["Error"];
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
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
            var inmuebles= repoInmueble.BuscarPorFechas(inicio,fin);
            if (inmuebles.Count == 0) {
                TempData["Error"] = "No hay inmuebles disponibles en esa fecha";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Inmuebles = inmuebles;
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
                    ViewBag.Inmuebles = repoInmueble.BuscarPorFechas(contrato.FechaInicio,contrato.FechaFin);
                    return View(contrato);
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(contrato);
            }
        }

        [Authorize]
        public ActionResult Renovar(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
            return View(entidad);
        }

            // GET: ContratoController/Edit/5
            [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
            ViewBag.Inquilinos = repoInquilino.ObtenerTodos();
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
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(contrato);
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            var inicio = entidad.FechaInicio;
            var final = entidad.FechaFin;
            var tiempoContrato = final - inicio;
            var hoy = DateTime.Now;
            if (final - hoy > tiempoContrato / 2)
            {
                ViewBag.Multa = entidad.Precio * 2;
            }
            else {
                ViewBag.Multa = entidad.Precio;
            }
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
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
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(contrato);
            }
        }
    }
}
