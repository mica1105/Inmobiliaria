using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Inmobiliaria.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly DataContext _context;

        public ContratoController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Contrato/5
        [HttpGet]
        public async Task<ActionResult<IList<Contrato>>> GetContrato()
        {
            try
            {
                var propietario = User.Identity.Name;
                var contratos = await _context.Contrato.Include(x => x.Inmueble).Include(x=>x.Inquilino)
                    .Where(x => x.Inmueble.Duenio.Email == propietario)
                    .ToListAsync();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Contrato/Vigentes
        [HttpGet("Vigentes")]
        public async Task<ActionResult<IList<Contrato>>> GetVigentes()
        {
            try
            {
                var propietario = User.Identity.Name;
                var contratos = await _context.Contrato.Include(x=>x.Inmueble).Include(x=> x.Inquilino)
                    .Where(x => x.Inmueble.Duenio.Email == propietario && x.FechaInicio <= DateTime.Now && x.FechaFin >= DateTime.Now)
                    .ToListAsync();
                return Ok(contratos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // GET: api/Contrato/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> GetContrato(int id)
        {
            try
            {
                var propietario = User.Identity.Name;
                var contrato = await _context.Contrato.Include(x => x.Inmueble).Include(x=>x.Inquilino)
                    .Where(x => x.Inmueble.Duenio.Email == propietario)
                    .SingleAsync();
                return Ok(contrato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // PUT: api/Contratoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContrato(int id, Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid && _context.Contrato.AsNoTracking().Include(e => e.Inmueble).ThenInclude(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Inmueble.Duenio.Email == User.Identity.Name) != null)
                {
                    contrato.Id = id;
                    _context.Contrato.Update(contrato);
                    await _context.SaveChangesAsync();
                    return Ok(contrato);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Contratoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Contrato>> PostContrato(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contrato.Inmueble.Duenio.Id = _context.Propietario.Single(e => e.Email == User.Identity.Name).Id;
                    _context.Contrato.Add(contrato);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetContrato", new { id = contrato.Id }, contrato);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Contratoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contrato>> DeleteContrato(int id)
        {
            try
            {
                var entidad = _context.Contrato.Include(e => e.Inmueble).ThenInclude(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Inmueble.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    _context.Contrato.Remove(entidad);
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

        private bool ContratoExists(int id)
        {
            return _context.Contrato.Any(e => e.Id == id);
        }
        public async Task<IActionResult> BajaLogica(int id)
        {
            try
            {
                var entidad = _context.Contrato.Include(e => e.Inmueble).ThenInclude(e => e.Duenio).FirstOrDefault(e => e.Id == id && e.Inmueble.Duenio.Email == User.Identity.Name);
                if (entidad != null)
                {
                    entidad.Precio = 0;//cambiar por estado = 0
                    _context.Contrato.Update(entidad);
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
