using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class PagoController : Controller
    {
        private IRepositorio<Contrato> repoContrato;
        private IRepositorioPago repositorio;
        public PagoController(IConfiguration configuration)
        {
            repositorio = new RepositorioPago(configuration);
            repoContrato = new RepositorioContrato(configuration);
        }

        // GET: PagoController
        [Authorize]
        public ActionResult Index(int id)
        {
            ViewBag.Contrato= repoContrato.ObtenerPorId(id);
            var lista = repositorio.ObtenerTodos(id);
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }
        
        // GET: PagoController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PagoController/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            var pagos = repositorio.ObtenerTodos(id);
            ViewBag.Pago= pagos.Count();
            ViewBag.Contrato = repoContrato.ObtenerPorId(id);
            return View();
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(pago);
                    TempData["Id"] = pago.Id;
                    return RedirectToAction("Index","Pago",new {id= pago.ContratoId});
                }
                else
                {
                    ViewBag.Contrato = repoContrato.ObtenerPorId(pago.ContratoId);
                    return View(pago);
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(pago);
            }
        }

        // GET: PagoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
                repositorio.Modificacion(pago);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction("Index", "Pago", new { id = pago.ContratoId });
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }

        // GET: PagoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            { 
                repositorio.Baja(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction("Index", "Contrato");
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(pago);
            }
        }
    }
}
