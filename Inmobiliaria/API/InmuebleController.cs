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
    public class InmuebleController : ControllerBase
    {
        private readonly DataContext _context;

        public InmuebleController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Inmueble
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inmueble>>> GetInmueble()
        {
            return await _context.Inmueble.ToListAsync();
        }

        // GET: api/Inmueble/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inmueble>> GetInmueble(int id)
        {
            var inmueble = await _context.Inmueble.FindAsync(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            return inmueble;
        }

        // PUT: api/Inmueble/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInmueble(int id, Inmueble inmueble)
        {
            if (id != inmueble.Id)
            {
                return BadRequest();
            }

            _context.Entry(inmueble).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InmuebleExists(id))
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

        // POST: api/Inmueble
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Inmueble>> PostInmueble(Inmueble inmueble)
        {
            _context.Inmueble.Add(inmueble);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInmueble", new { id = inmueble.Id }, inmueble);
        }

        // DELETE: api/Inmueble/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inmueble>> DeleteInmueble(int id)
        {
            var inmueble = await _context.Inmueble.FindAsync(id);
            if (inmueble == null)
            {
                return NotFound();
            }

            _context.Inmueble.Remove(inmueble);
            await _context.SaveChangesAsync();

            return inmueble;
        }

        private bool InmuebleExists(int id)
        {
            return _context.Inmueble.Any(e => e.Id == id);
        }
    }
}
