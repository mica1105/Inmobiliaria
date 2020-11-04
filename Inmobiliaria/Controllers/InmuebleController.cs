using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IWebHostEnvironment environment;
        private readonly IRepositorioPropietario repoPropietario;
        public InmuebleController(IRepositorioInmueble repositorio, IWebHostEnvironment environment, IRepositorioPropietario repositorioPropietario)
        {
            this.repositorio = repositorio;
            this.environment = environment;
            repoPropietario = repositorioPropietario;
        }

        // GET: InmuebleController
        [Authorize]
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }

        public ActionResult Disponibles(IFormCollection collection)
        {
            try
            {
                DateTime desde = DateTime.Parse(collection["Desde"]);
                DateTime hasta = DateTime.Parse(collection["Hasta"]);
                var lista = repositorio.BuscarPorFechas(desde,hasta);
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        public ActionResult Activos()
        {
            try
            {
                var lista = repositorio.ObtenerPorEstado();
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        [Authorize]
        public ActionResult InmueblesPorPropietario(int id)
        {
            try
            {
                var lista = repositorio.BuscarPorPropietario(id);
                ViewBag.Id = TempData["Id"];
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View();
            }
        }

        // GET: InmuebleController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            ViewBag.Estados = Inmueble.ObtenerEstados();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                    int res = repositorio.Alta(inmueble);
                    if (inmueble.ImagenFile != null && inmueble.Id > 0)
                    {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                        string fileName = "inmueble_" + inmueble.Id + Path.GetExtension(inmueble.ImagenFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        inmueble.Imagen = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            inmueble.ImagenFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(inmueble);
                    }
                    TempData["Id"] = inmueble.Id;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                    return View(inmueble);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(inmueble);
            }
        }

        // GET: InmuebleController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
            ViewBag.Propietarios = repoPropietario.ObtenerTodos();
            ViewBag.Estados = Inmueble.ObtenerEstados();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(entidad);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                repositorio.Modificacion(inmueble);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(inmueble);
            }
        }

        // GET: InmuebleController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var i = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
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
                return View(inmueble);
            }
        }

        [Authorize]
        public IActionResult Imagen(int id)
        {

            try
            {
                var i = repositorio.ObtenerPorId(id);
                var stream = System.IO.File.Open(
                    Path.Combine(environment.WebRootPath, i.Imagen.Substring(1)),
                    FileMode.Open,
                    FileAccess.Read);
                var ext = Path.GetExtension(i.Imagen);
                return new FileStreamResult(stream, $"image/{ext.Substring(1)}");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
        }
    }
}
