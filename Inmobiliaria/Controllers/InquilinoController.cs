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
		private readonly IRepositorio<Inquilino> repositorio;
        public InquilinoController(IRepositorio<Inquilino> repositorio)
        {
            this.repositorio = repositorio;
        }


        // GET: Inquilino
        [Authorize]
        public ActionResult Index()
        {
			var lista = repositorio.ObtenerTodos();
			return View(lista);
        }

        // GET: Inquilino/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                var e = repositorio.ObtenerPorId(id);
                return View(e);
            }
            catch (Exception ex)
            {
                throw;
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
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {          
					repositorio.Alta(inquilino);
					return RedirectToAction(nameof(Index));
			}
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Inquilino/Edit
        [Authorize]
        public ActionResult Edit(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        // POST: Inquilino/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
          
            try
            {
                repositorio.Modificacion(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Inquilino/Delete
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        // POST: Inquilino/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino inquilino)
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