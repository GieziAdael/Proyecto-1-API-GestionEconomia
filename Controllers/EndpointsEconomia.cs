using API_GestionEconomia.DataBase;
using API_GestionEconomia.DataBase.Entities;
using API_GestionEconomia.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_GestionEconomia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EndpointsEconomia : ControllerBase
    {
        private readonly MyAppDbContext _context;

        public EndpointsEconomia(MyAppDbContext context)
        {
            _context = context;
        }

        //Ver todas las economias por Codigo
        [HttpGet("verEconomiasPorCodigo/{codigoOrganizacion}/{IdUsuario}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Economia>>> GetAllHistorialOrg
            (string codigoOrganizacion, int IdUsuario)
        {
            try
            {
                var eresMiembro = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKCodigoEconomia == codigoOrganizacion && u.FKIdUsuario == IdUsuario);
                if (eresMiembro is null)
                {
                    return NotFound("No se encontró el usuario organizador con el ID proporcionado.");
                }
                if (eresMiembro.tipoUsuario != "Owner" && eresMiembro.tipoUsuario != "Admin" && eresMiembro.tipoUsuario != "View")
                {
                    return BadRequest("No tienes permiso para ver esta organizacion.");
                }
                var registros = await _context.Economias.Where(u => u.FKCodigoEconomia == codigoOrganizacion).ToListAsync();
                return Ok(registros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Crear nuevo movimiento economico
        [HttpPost("crearMovimientoEconomico")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Economia>> PostNewMovimientoEconomico
            (string codigoOrganizacion, int IdUsuario, [FromBody] DtoHistorialEconomia dtoHistorialEconomia)
        {
            try
            {

                var puedesCrearMovimiento = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKCodigoEconomia == codigoOrganizacion && u.FKIdUsuario == IdUsuario);
                if (puedesCrearMovimiento is null)
                {
                    return NotFound("No se encontró el usuario organizador con el ID proporcionado.");
                }
                if (puedesCrearMovimiento.tipoUsuario != "Owner" && puedesCrearMovimiento.tipoUsuario != "Admin")
                {
                    return BadRequest("No tienes permiso para agregar movimientos a esta organización económica.");
                }

                if (dtoHistorialEconomia.DescripcionMovimiento is null)
                {
                    return BadRequest("La descripción del movimiento no puede estar vacía");
                }
                var nRegistros = await _context.Economias.Where(u => u.FKCodigoEconomia == codigoOrganizacion).ToListAsync();
                int nMovimiento = 1;
                foreach (var r in nRegistros)
                {
                    nMovimiento += 1;
                }
                var newMovimiento = new Economia
                {
                    NumMovimiento = nMovimiento,
                    DescripcionMovimiento = dtoHistorialEconomia.DescripcionMovimiento,
                    IngresoEgreso = dtoHistorialEconomia.IngresoEgreso,
                    FechaMovimiento = DateTime.UtcNow,
                    FKCodigoEconomia = codigoOrganizacion
                };
                _context.Economias.Add(newMovimiento);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(PostNewMovimientoEconomico), new { id = newMovimiento.Id }, newMovimiento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Calcular balance actual
        [HttpGet("calcularBalanceActual/{codigoOrganizacion}/{IdUsuario}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetCalcularBalanceActual
            (string codigoOrganizacion, int IdUsuario)
        {
            try
            {
                var eresMiembro = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKCodigoEconomia == codigoOrganizacion && u.FKIdUsuario == IdUsuario);
                if (eresMiembro is null)
                {
                    return NotFound("No se encontró el usuario organizador con el ID proporcionado.");
                }
                if (eresMiembro.tipoUsuario != "Owner" && eresMiembro.tipoUsuario != "Admin" && eresMiembro.tipoUsuario != "View")
                {
                    return BadRequest("No tienes permiso para ver esta organizacion.");
                }
                var registros = await _context.Economias.Where(u => u.FKCodigoEconomia == codigoOrganizacion).ToListAsync();
                decimal balance = 0;
                foreach (var movimiento in registros)
                {
                    balance += movimiento.IngresoEgreso;
                }
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Eliminar movimiento economico
        [HttpDelete("eliminarMovimientoEconomico/{codigoOrganizacion}/{IdUsuario}/{NumMovimiento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Economia>> DeleteMov(string codigoOrganizacion, int IdUsuario, int NumMovimiento)
        {
            try
            {
                var eresMiembro = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKCodigoEconomia == codigoOrganizacion && u.FKIdUsuario == IdUsuario);
                if (eresMiembro is null)
                {
                    return NotFound("No se encontró el usuario organizador con el ID proporcionado.");
                }
                if (eresMiembro.tipoUsuario != "Owner" && eresMiembro.tipoUsuario != "Admin" && eresMiembro.tipoUsuario != "View")
                {
                    return BadRequest("No tienes permiso para ver esta organizacion.");
                }
                var registros = await _context.Economias.Where(u => u.FKCodigoEconomia == codigoOrganizacion).ToListAsync();
                int nRegistros = 0;
                foreach (var r in registros)
                {
                    nRegistros += 1;
                }
                if (NumMovimiento > nRegistros || NumMovimiento < 1)
                {
                    return BadRequest("El número de movimiento proporcionado no es válido.");
                }
                var movimientoAEliminar = await _context.Economias.FirstOrDefaultAsync(m => m.FKCodigoEconomia == codigoOrganizacion && m.NumMovimiento == NumMovimiento);
                if (movimientoAEliminar is null)
                {
                    return NotFound("No se encontró el movimiento económico con el número proporcionado.");
                }
                _context.Economias.Remove(movimientoAEliminar);
                await _context.SaveChangesAsync();
                return Ok("Movimiento económico eliminado correctamente.");
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Borrar todas las economias por Codigo
        [HttpDelete("borrarHistorialEconomico/{codigoOrganizacion}/{IdUsuario}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Economia>>> DeleteAllHistorialOrg
            (string codigoOrganizacion, int IdUsuario)
        {
            try
            {
                var eresMiembro = await _context.Organizadores.FirstOrDefaultAsync(u => u.FKCodigoEconomia == codigoOrganizacion && u.FKIdUsuario == IdUsuario);
                if (eresMiembro is null)
                {
                    return NotFound("No se encontró el usuario organizador con el ID proporcionado.");
                }
                if (eresMiembro.tipoUsuario != "Owner" && eresMiembro.tipoUsuario != "Admin")
                {
                    return BadRequest("No tienes permiso para borrar el historial economico de esta Organizacion.");
                }
                var registros = await _context.Economias.Where(u => u.FKCodigoEconomia == codigoOrganizacion).ToListAsync();
                if (registros.Count == 0)
                {
                    return NotFound("No se encontraron registros económicos para la organización proporcionada.");
                }
                foreach (var registro in registros)
                {
                    _context.Economias.Remove(registro);
                }
                await _context.SaveChangesAsync();
                return Ok("Historial económico borrado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
