﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Inmobiliaria.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InquilinoController : ControllerBase
    {
        private readonly DataContext _context;

        public InquilinoController(DataContext context)
        {
            _context = context;
        }
        // GET: api/Inquilino
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inquilino>>> GetInquilino()
        {
            try
            {
                var lista= await _context.Inquilino.ToListAsync();
                return Ok(lista);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Inquilino
        [HttpGet("PorPropietario")]
        public async Task<ActionResult> GetPorPropietario()
        {
            try
            {
                //return await _context.Inquilino.ToListAsync();
                var propietario = User.Identity.Name;
                var res = await _context.Contrato
                        .Include(x => x.Inquilino)
                        .Where(x => x.Inmueble.Duenio.Email == propietario)
                        .Select(x => x.Inquilino)
                        .ToListAsync();
                return Ok(res);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // GET: api/Inquilino/Inmueble/5
        [HttpGet("Inmueble/{id}")]
        public async Task<ActionResult> GetInmueble(int id)
        {
            try
            {
                //return await _context.Inquilino.ToListAsync();
                var propietario = User.Identity.Name;
                var res = await _context.Contrato
                        .Include(x => x.Inmueble)
                        .Where(x => x.Inmueble.Duenio.Email == propietario && x.InquilinoId == id)
                        .Select(x => x.Inmueble.Direccion)
                        .FirstAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Inquilino/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inquilino>> GetInquilino(int id)
        {
            try
            {
                var inquilino = await _context.Inquilino.FindAsync(id);

                if (inquilino == null)
                {
                    return NotFound();
                }

                return inquilino;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Inquilino/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInquilino(int id, Inquilino inquilino)
        {
            if (id != inquilino.Id)
            {
                return BadRequest();
            }

            _context.Entry(inquilino).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InquilinoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Inquilino
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Inquilino>> PostInquilino(Inquilino inquilino)
        {
            _context.Inquilino.Add(inquilino);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInquilino", new { id = inquilino.Id }, inquilino);
        }

        // DELETE: api/Inquilino/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inquilino>> DeleteInquilino(int id)
        {
            var inquilino = await _context.Inquilino.FindAsync(id);
            if (inquilino == null)
            {
                return NotFound();
            }

            _context.Inquilino.Remove(inquilino);
            await _context.SaveChangesAsync();

            return inquilino;
        }

        private bool InquilinoExists(int id)
        {
            return _context.Inquilino.Any(e => e.Id == id);
        }
    }
}
