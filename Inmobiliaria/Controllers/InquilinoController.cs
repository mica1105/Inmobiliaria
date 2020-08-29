using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class InquilinoController : Controller
    {
		RepositorioInquilino repositorio;

		public InquilinoController()
		{
		  repositorio= new RepositorioInquilino();
		}

        // GET: Inquilino
        public ActionResult Index()
        {
			var lista = repositorio.ObtenerTodos();
			return View(lista);
        }

        // GET: Inquilino/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino propietario)
        {
            try
            {          
					repositorio.Alta(propietario);
					return RedirectToAction(nameof(Index));
			}
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Inquilino/Edit
        public ActionResult Edit(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        // POST: Inquilino/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino e = null;
            try
            {
                e = repositorio.ObtenerPorId(id);
                e.Nombre = collection["Nombre"];
                e.Apellido = collection["Apellido"];
                e.Dni = collection["Dni"];
                e.LugarTrabajo = collection["LugarTrabajo"];
                e.Email = collection["Email"];
                e.Telefono = collection["Telefono"];
                e.NombreGarante = collection["NombreGarante"];
                e.DniGarante = collection["DniGarante"];
                e.TelefonoGarante = collection["TelefonoGarante"];
                repositorio.Modificacion(e);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Inquilino/Delete
        public ActionResult Delete(int id)
        {
            var e = repositorio.ObtenerPorId(id);
            return View(e);
        }

        // POST: Inquilino/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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