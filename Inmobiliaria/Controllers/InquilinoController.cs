﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Controllers
{
    public class InquilinoController : Controller
    {
		RepositorioInquilino repositorio;
        public InquilinoController(IConfiguration configuration)
        {
            repositorio = new RepositorioInquilino(configuration);
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