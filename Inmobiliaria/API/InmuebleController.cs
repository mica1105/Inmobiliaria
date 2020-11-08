using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Inmobiliaria.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmuebleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration conf;

        public InmuebleController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            conf = configuration;
        }

        // GET: api/Inmueble
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inmueble>>> GetInmueble()
        {
            try
            {
                var usuario = User.Identity.Name;
                var res = await _context.Inmueble.Where(e => e.Duenio.Email == usuario).ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Inmueble/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inmueble>> GetInmueble(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var inmueble = await _context.Inmueble.Include(e => e.Duenio).Where(e => e.Duenio.Email == usuario).SingleAsync(e => e.Id == id);
                return Ok(inmueble);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("PorInquilino/{id}")]
        public async Task<ActionResult<Inmueble>> GetPorInquilino(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var inmueble = await _context.Contrato.Include(e => e.Inquilino)
                    .Where(e => e.Inmueble.Duenio.Email == usuario && e.Inquilino.Id == id)
                    .Select(x => x.Inmueble).SingleAsync();
                return Ok(inmueble);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // PUT: api/Inmueble/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInmueble(int id, Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid && _context.Inmueble.AsNoTracking().Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name) != null)
                {
                    inmueble.Id = id;
                    _context.Inmueble.Update(inmueble);
                    await _context.SaveChangesAsync();
                    return Ok(inmueble);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Inmueble
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Inmueble>> PostInmueble(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                inmueble.PropietarioId = _context.Propietario.Single(e => e.Email == User.Identity.Name).Id;
                _context.Inmueble.Add(inmueble);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetInmueble", new { id = inmueble.Id }, inmueble);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Inmueble/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inmueble>> DeleteInmueble(int id)
        {
            try
            {
                var entidad = _context.Inmueble.Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    _context.Inmueble.Remove(entidad);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        // DELETE api/<controller>/5
        [HttpDelete("BajaLogica/{id}")]
        public async Task<IActionResult> BajaLogica(int id)
        {
            try
            {
                var entidad = _context.Inmueble.Include(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    entidad.Estado = 0;//cambiar por estado = 0
                    _context.Inmueble.Update(entidad);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
