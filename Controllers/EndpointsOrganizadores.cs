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
    public class EndpointsOrganizadores : ControllerBase
    {
        private readonly MyAppDbContext _context;

        public EndpointsOrganizadores(MyAppDbContext context)
        {
            _context = context;
        }

        //Añadir a un nuevo organizador por CodigoOrganizacion
        [HttpPost("NuevoOrganizador/{codigOrganizacion}/{codigoOwner}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Organizador>> PostNewOrganizador
            (string codigOrganizacion, string codigoOwner, [FromBody] DtoOrganizador dtoOrganizador)
        {
            try
            {
                var eresOwner = await _context.OrganizacionesEconomicas.FirstOrDefaultAsync
                    (u => u.CodigoEconomia == codigOrganizacion && u.FKCodigoUsuario == codigoOwner);
                if (eresOwner is null)
                {
                    return BadRequest("No tienes permiso para añadir organizadores en tu contexto actual.");
                }
                if (dtoOrganizador.Email is null)
                {
                    return BadRequest("El email no puede estar vacío");
                }
                if (dtoOrganizador.TipoUsuario is null)
                {
                    return BadRequest("El tipo de usuario no puede estar vacío");
                }
                if (dtoOrganizador.TipoUsuario == "Owner")
                {
                    return BadRequest("No puedes asignar el rol 'Owner' a otro usuario, tu eres el unico Owner.");
                }
                if ( dtoOrganizador.TipoUsuario != "Admin" && dtoOrganizador.TipoUsuario != "View")
                {
                    return BadRequest("El tipo de usuario debe ser 'Admin' o 'View'");
                }
                var emailNormalizado = Normalizar.NormalizarEmail(dtoOrganizador.Email);
                var obtenerUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailNormalizado);
                if (obtenerUsuario is null)
                {
                    return NotFound("No se encontró el usuario con el email proporcionado.");
                }
                var yaEsOrganizador = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKIdUsuario == obtenerUsuario.Id && u.FKCodigoEconomia == codigOrganizacion);
                if (yaEsOrganizador is not null)
                {
                    return Conflict("El usuario ya es organizador en este contexto.");
                }
                var newOrganizador = new Organizador
                {
                    tipoUsuario = dtoOrganizador.TipoUsuario,
                    FKCodigoEconomia = codigOrganizacion,
                    FKIdUsuario = obtenerUsuario.Id
                };
                _context.Organizadores.Add(newOrganizador);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(PostNewOrganizador), new { id = newOrganizador.Id }, newOrganizador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Editar tipo de organizador
        [HttpPut("EditarOrganizador/{codigOrganizacion}/{codigoOwner}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Organizador>> PutEditarOrganizador
            (string codigOrganizacion, string codigoOwner, [FromBody] DtoOrganizador dtoOrganizador)
        {
            var eresOwner = await _context.OrganizacionesEconomicas.FirstOrDefaultAsync
                (u => u.CodigoEconomia == codigOrganizacion && u.FKCodigoUsuario == codigoOwner);
            if (eresOwner is null)
            {
                return BadRequest("No tienes permiso para añadir organizadores en tu contexto actual.");
            }
            if (dtoOrganizador.Email is null)
            {
                return BadRequest("El email no puede estar vacío");
            }
            if (dtoOrganizador.TipoUsuario is null)
            {
                return BadRequest("El tipo de usuario no puede estar vacío");
            }
            if (dtoOrganizador.TipoUsuario == "Owner")
            {
                return BadRequest("No puedes asignar el rol 'Owner' a otro usuario, tu eres el unico Owner.");
            }
            if (dtoOrganizador.TipoUsuario != "Admin" && dtoOrganizador.TipoUsuario != "View")
            {
                return BadRequest("El tipo de usuario debe ser 'Admin' o 'View'");
            }
            var emailNormalizado = Normalizar.NormalizarEmail(dtoOrganizador.Email);
            var obtenerUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailNormalizado);
            if (obtenerUsuario is null)
            {
                return NotFound("No se encontró el usuario con el email proporcionado.");
            }
            var organizador = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKIdUsuario == obtenerUsuario.Id && u.FKCodigoEconomia == codigOrganizacion);
            if (organizador is null)
            {
                return NotFound("No se encontró el organizador con el email proporcionado en este contexto.");
            }
            //Evitar que el Owner se quite a si mismo su propio rol
            if (obtenerUsuario.CodigoUsuario == codigoOwner)
            {
                return BadRequest("No puedes cambiar tu propio rol de Owner.");
            }

            organizador.tipoUsuario = dtoOrganizador.TipoUsuario;
            _context.Organizadores.Update(organizador);
            await _context.SaveChangesAsync();
            return Ok(organizador);
        }

        //Eliminar organizador
        [HttpDelete("EliminarOrganizador/{codigOrganizacion}/{codigoOwner}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Organizador>> DeleteOrganizador
            (string codigOrganizacion, string codigoOwner, [FromBody] string email)
        {
            var eresOwner = await _context.OrganizacionesEconomicas.FirstOrDefaultAsync
                (u => u.CodigoEconomia == codigOrganizacion && u.FKCodigoUsuario == codigoOwner);
            if (eresOwner is null)
            {
                return BadRequest("No tienes permiso para añadir organizadores en tu contexto actual.");
            }
            var obtenerUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.CodigoUsuario == codigoOwner);
            if (obtenerUsuario is null)
            {
                return NotFound("No se encontró el usuario con el código proporcionado.");
            }
            

            var emailNormalizado = Normalizar.NormalizarEmail(email);
            var organizador = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailNormalizado);
            if (organizador is null)
            {
                return NotFound("No se encontró el usuario con el email proporcionado.");
            }
            
            var codigo = organizador.CodigoUsuario;

            //Evitar que el Owner se quite a si mismo su propio rol
            if (codigo == codigoOwner)
            {
                return BadRequest("No puedes eliminar tu propio rol de Owner.");
            }
            var esOrganizador = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKIdUsuario == organizador.Id && u.FKCodigoEconomia == codigOrganizacion);
            if (esOrganizador is null)
            {
                return NotFound("No se encontró el organizador con el email proporcionado en este contexto.");
            }

            _context.Organizadores.Remove(esOrganizador);
            await _context.SaveChangesAsync();
            return Ok(esOrganizador);

        }
    }
}
