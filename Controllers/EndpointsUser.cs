using API_GestionEconomia.DataBase;
using API_GestionEconomia.DataBase.Entities;
using API_GestionEconomia.Dtos;
using API_GestionEconomia.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_GestionEconomia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EndpointsUser : ControllerBase
    {
        private readonly MyAppDbContext _context;

        public EndpointsUser(MyAppDbContext context)
        {
            _context = context;
        }

        //Crear nuevo usuario
        [HttpPost("crearUsuario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Usuario>> PostNewUser([FromBody] DtoUser dtoUser)
        {
            try
            {
                //1
                if (dtoUser.Email is null)
                {
                    return BadRequest("El email no puede estar vacío");
                }

                var emailNormalizado = Normalizar.NormalizarEmail(dtoUser.Email);
                var existingUser = _context.Usuarios.FirstOrDefault(u => u.Email == emailNormalizado);

                if (existingUser is not null)
                {
                    return Conflict("El email ya está registrado");
                }

                if (dtoUser.Email.Length < 8)
                {
                    return BadRequest("El email debe tener al menos 8 caracteres");
                }
                if (dtoUser.Password is null)
                {
                    return BadRequest("La contraseña no puede estar vacía");
                }
                if(dtoUser.Password.Length < 8)
                {
                    return BadRequest("La contraseña debe tener al menos 8 caracteres");
                }

                //2
                var hashPassw = PasswordHasher.Hash(dtoUser.Password);
                var codigoUsuario = CreadorCodigos.GenerarCodigoAleatorio();

                var newUser = new Usuario
                {
                    Email = emailNormalizado,
                    Password_Hash = hashPassw,
                    CodigoUsuario = codigoUsuario
                };

                //3
                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(PostNewUser), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Inicio de sesión de usuario
        [HttpPost("loginUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Usuario>> GetLoginUser([FromBody] DtoUser dtoUser)
        {
            try
            {
                //1
                if (dtoUser.Email is null)
                {
                    return BadRequest("El email no puede estar vacío");
                }
                if (dtoUser.Password is null)
                {
                    return BadRequest("La contraseña no puede estar vacía");
                }

                var emailNormalizado = Normalizar.NormalizarEmail(dtoUser.Email);
                var passwordPlano = dtoUser.Password;
                //2
                var registro = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailNormalizado);
                if (registro is null)
                {
                    return Unauthorized("Credenciales inválidas");
                }
                bool isPasswordValid = PasswordHasher.Verify(passwordPlano, registro.Password_Hash);
                if (!isPasswordValid)
                {
                    return Unauthorized("Credenciales inválidas");
                }

                //3
                return Ok(registro.CodigoUsuario);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Eliminar cuenta de usuario
        [HttpDelete("eliminarUsuario/{codigoUsuario}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Usuario>> DeleteUser(string codigoUsuario)
        {
            try
            {
                //Verificar si el usuario existe
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CodigoUsuario == codigoUsuario);
                if (usuario is null)
                {
                    return NotFound("No se encontró el usuario con el código proporcionado.");
                }
                //Eliminar las organizaciones económicas asociadas al usuario
                var registrosOrgEconm = await _context.OrganizacionesEconomicas.Where(o => o.FKCodigoUsuario == codigoUsuario).ToListAsync();
                foreach (var r in registrosOrgEconm)
                {
                    //Eliminar los organizadores asociados a la organización económica
                    var organizadores = await _context.Organizadores.Where(o => o.FKCodigoEconomia == r.CodigoEconomia).ToListAsync();
                    if(organizadores is not null)
                    {
                        _context.Organizadores.RemoveRange(organizadores);
                        await _context.SaveChangesAsync();
                    }

                    //Eliminar los registros económicos asociados a la organización económica
                    var registrosEconm = await _context.Economias.Where(e => e.FKCodigoEconomia == r.CodigoEconomia).ToListAsync();
                    if (registrosEconm is not null)
                    {
                        _context.Economias.RemoveRange(registrosEconm);
                        await _context.SaveChangesAsync();
                    }

                    _context.OrganizacionesEconomicas.Remove(r);
                    await _context.SaveChangesAsync();
                }
                //Eliminar cuenta de usuario
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
