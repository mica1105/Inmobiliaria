using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;
using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropietarioController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration config;

        public PropietarioController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            config = configuration;
        }

        // GET: api/Propietario
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try {
                var propietario = User.Identity.Name;
                var res = await _context.Propietario.Select(x=> new { x.Nombre, x.Apellido, x.Dni, x.Email}).SingleOrDefaultAsync(x=> x.Email== propietario);
                return Ok(res);
            } 
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // POST: api/Propietario
        [HttpPost]
        public async Task<ActionResult<Propietario>> Post(Propietario propietario)
        {
            try {
                if (ModelState.IsValid) {
                    _context.Propietario.Add(propietario);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetPropietario", new { id = propietario.Id }, propietario);
                }
                return BadRequest();
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }

        // PUT: api/Propietario/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Inmueble entidad)
        {
            try
            {
                if (ModelState.IsValid && _context.Inmueble.AsNoTracking().Include(e=> e.Duenio).FirstOrDefault(e=>e.Id== id && e.Duenio.Email == User.Identity.Name) != null) {

                    entidad.Id = id;
                    _context.Inmueble.Update(entidad);
                    await _context.SaveChangesAsync();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        

        // DELETE: api/Propietario/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Propietario>> Delete(int id)
        {
            var propietario = await _context.Propietario.FindAsync(id);
            if (propietario == null)
            {
                return NotFound();
            }

            _context.Propietario.Remove(propietario);
            await _context.SaveChangesAsync();

            return propietario;
        }

        private bool PropietarioExists(int id)
        {
            return _context.Propietario.Any(e => e.Id == id);
        }
    }
}
