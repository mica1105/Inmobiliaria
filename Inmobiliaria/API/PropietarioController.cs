using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Inmobiliaria.API
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietario()
        {
            var res= await _context.Propietario
                    .Select(x => new { x.Id, x.Nombre, x.Apellido, x.Dni, x.Email})
                    .ToListAsync();
            return Ok(res);
        }*/

        [HttpGet("Vigentes")]
        public async Task<ActionResult<IEnumerable<Propietario>>> GetVigentes()
        {
            try
            {
                return await _context.Contrato
                    .Include(x => x.Inmueble)
                    .ThenInclude(x=> x.Duenio)
                    .Where(x => x.FechaInicio <= DateTime.Now && x.FechaFin >= DateTime.Now)
                    .Select(x => x.Inmueble.Duenio)
                    .AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Propietario/5
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var propietario = User.Identity.Name;
                var res = await _context.Propietario.Select(x => new { x.Id, x.Nombre, x.Apellido, x.Dni, x.Email }).SingleOrDefaultAsync(x => x.Email == propietario);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = _context.Propietario.FirstOrDefault(x => x.Email == loginView.Usuario);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Email),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Propietario/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,[FromForm] Propietario entidad)
        {
            try
            {
                if (ModelState.IsValid && _context.Propietario.AsNoTracking().FirstOrDefault(x=> x.Email == User.Identity.Name) != null)
                {
                    entidad.Id = id;

                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: entidad.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                    entidad.Clave = hashed;

                    _context.Propietario.Update(entidad) ;
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

            // POST: api/Propietario
            // To protect from overposting attacks, enable the specific properties you want to bind to, for
            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Propietario>> Post([FromForm] Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Propietario.Add(propietario);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetPropietario", new { id = propietario.Id }, propietario);
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
        public async Task<ActionResult<Propietario>> DeletePropietario(int id)
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
