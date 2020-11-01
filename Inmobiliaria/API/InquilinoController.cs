using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;

namespace Inmobiliaria.API
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult> GetInquilino()
        {
            //return await _context.Inquilino.ToListAsync();
            var propietario = User.Identity.Name;
            var res = await _context.Contrato
                    .Include(x=>x.Inquilino)
                    .Include(x => x.Inmueble)
                    .ThenInclude(x=> x.Duenio)
                    .Where(x => x.Inmueble.Duenio.Email == propietario)
                    .Select(x => new { x.Inquilino, x.Inmueble.Direccion })
                    .ToListAsync();
            return Ok(res);
        }

        // GET: api/Inquilino/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inquilino>> GetInquilino(int id)
        {
            var inquilino = await _context.Inquilino.FindAsync(id);

            if (inquilino == null)
            {
                return NotFound();
            }

            return inquilino;
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
