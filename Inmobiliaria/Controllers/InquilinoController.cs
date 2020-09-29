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
    public class InquilinoController : Controller
    {
		private readonly IRepositorioInquilino repositorio;
        public InquilinoController(IRepositorioInquilino repositorio)
        {
            this.repositorio = repositorio;
        }


        // GET: Inquilino
        [Authorize]
        public ActionResult Index()
        {                
                var lista = repositorio.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
            
        }

        [Authorize]
        public ActionResult Buscar(string q)
        {
            try
            {
                var lista = repositorio.BuscarPorNombre(q);
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (lista.Count == 0)
                {
                    TempData["Mensaje"] = "No se han encontrado propietarios con ese nombre";
                    return RedirectToAction(nameof(Index));
                 
                }
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        // GET: Inquilino/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var e = new Inquilino();
            try
            {
                e = repositorio.ObtenerPorId(id);
                return View(e);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(e);
            }
         
        }

        // GET: Inquilino/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(inquilino);
                    TempData["Id"] = inquilino.Id;
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return View(inquilino);
                }

			}
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inquilino);
            }
        }

        // GET: Inquilino/Edit
        [Authorize]
        public ActionResult Edit(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(e);
        }

        // POST: Inquilino/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
          
            try
            {
                repositorio.Modificacion(inquilino);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(inquilino);
            }
        }

        // GET: Inquilino/Delete
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(e);
        }

        // POST: Inquilino/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino inquilino)
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
                return View(inquilino);
            }
        }
    }
}