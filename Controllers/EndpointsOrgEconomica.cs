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
    public class EndpointsOrgEconomica : ControllerBase
    {
        private readonly MyAppDbContext _context;

        public EndpointsOrgEconomica(MyAppDbContext context)
        {
            _context = context;
        }

        //Crear nueva organizacion economica
        [HttpPost("crearOrganizacionEconomica")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PostNewOrgEconomica([FromBody] DtoOrgEconm dtoOrgEconm)
        {
            try
            {
                //1
                if (dtoOrgEconm.NombreOrg is null || dtoOrgEconm.PasswordOrg is null)
                {
                    return BadRequest("El nombre de la organización o contraseña, no puede estar vacío");
                }
                var existingOrg = _context.OrganizacionesEconomicas.FirstOrDefault(o => o.NombreOrg == dtoOrgEconm.NombreOrg);
                if (existingOrg is not null)
                {
                    return Conflict("El nombre de la organización ya está registrado");
                }
                if (dtoOrgEconm.NombreOrg.Length < 8)
                {
                    return BadRequest("El nombre de la organización debe tener al menos 8 caracteres");
                }
                if (dtoOrgEconm.PasswordOrg.Length < 8)
                {
                    return BadRequest("La contraseña debe tener al menos 8 caracteres");
                }
                //2
                var hashPassw = PasswordHasher.Hash(dtoOrgEconm.PasswordOrg);
                var codigoEconomia = CreadorCodigos.GenerarCodigoAleatorio();
                var codigoUsuario = dtoOrgEconm.CodigoUsuario;
                var newOrgEconm = new OrganizacionEconomica
                {
                    NombreOrg = dtoOrgEconm.NombreOrg,
                    PasswordOrg_Hash = hashPassw,
                    CodigoEconomia = codigoEconomia,
                    FKCodigoUsuario = codigoUsuario
                };

                _context.OrganizacionesEconomicas.Add(newOrgEconm);
                await _context.SaveChangesAsync();
                //3
                var obtenerUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CodigoUsuario == codigoUsuario);
                if (obtenerUsuario is null)
                {
                    return NotFound("No se encontró el usuario asociado al código proporcionado.");
                }
                _context.Organizadores.Add(new Organizador
                {
                    tipoUsuario = "Owner",
                    FKCodigoEconomia = newOrgEconm.CodigoEconomia,
                    FKIdUsuario = obtenerUsuario.Id

                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostNewOrgEconomica), new { id = newOrgEconm.Id }, newOrgEconm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");

            }
        }

        //Inicio de sesión organizacion economica
        [HttpPost("loginOrganizacionEconomica")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrganizacionEconomica>> PostLoginOrgEconm([FromBody] DtoAccederOrgEconm dtoAccederOrgEconm)
        {
            try
            {
                //1
                if (dtoAccederOrgEconm.NombreOrg is null || dtoAccederOrgEconm.PasswordOrg is null)
                {
                    return BadRequest("El nombre de la organización o la contraseña no pueden estar vacíos");
                }
                var nombreOrg = dtoAccederOrgEconm.NombreOrg;
                var passwordOrg = dtoAccederOrgEconm.PasswordOrg;
                var orgEconm = await _context.OrganizacionesEconomicas.FirstOrDefaultAsync(o => o.NombreOrg == nombreOrg);
                if (orgEconm is null)
                {
                    return NotFound("Credenciales inválidas");
                }
                //2
                var isPasswordValid = PasswordHasher.Verify(passwordOrg, orgEconm.PasswordOrg_Hash);
                if (!isPasswordValid)
                {
                    return Unauthorized("Credenciales inválidas");
                }
                //3
                var codigoEconomia = orgEconm.CodigoEconomia;
                var tienePermiso = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKCodigoEconomia == codigoEconomia);
                if (tienePermiso is null)
                {
                    return Unauthorized("El usuario no tiene permiso para acceder a esta organización económica.");
                }

                return Ok(new { codigo = codigoEconomia, IdQuienAccede = tienePermiso.FKIdUsuario });
                //return Ok(codigoEconomia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //Eliminar organizacion economica
        [HttpDelete("eliminarOrganizacionEconomica/{codigoOrganizacion}/{codigoOwner}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrganizacionEconomica>> DeleteOrganizacionEconomica
            (string codigoOrganizacion, string codigoOwner)
        {
            try
            {
                var eresOwner = await _context.OrganizacionesEconomicas.FirstOrDefaultAsync
                (u => u.CodigoEconomia == codigoOrganizacion && u.FKCodigoUsuario == codigoOwner);
                if (eresOwner is null)
                {
                    return BadRequest("No tienes permiso para eliminar esta organizacion en tu contexto actual.");
                }

                //Eliminar todos los organizadores asociados a la organizacion economica

                var registrosOrganizadores = await _context.Organizadores.Where
                    (u => u.FKCodigoEconomia == codigoOrganizacion).ToListAsync();
                if (registrosOrganizadores is not null)
                {
                    _context.Organizadores.RemoveRange(registrosOrganizadores);
                    await _context.SaveChangesAsync();
                }

                //Eliminar todos los movimientos economicos asociados a la organizacion economica
                var registrosMovimientos = await _context.Economias.Where
                    (u => u.FKCodigoEconomia == codigoOrganizacion).ToListAsync();
                if (registrosMovimientos is not null)
                {
                    _context.Economias.RemoveRange(registrosMovimientos);
                    await _context.SaveChangesAsync();
                }

                //Eliminar la organizacion economica
                _context.OrganizacionesEconomicas.Remove(eresOwner);
                await _context.SaveChangesAsync();

                return Ok(eresOwner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
