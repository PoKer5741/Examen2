using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteLab.Api.Data;
using SoporteLab.Api.Models;

namespace SoporteLab.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly SoporteDbContext _context;

        public TicketsController(SoporteDbContext context)
        {
            _context = context;
        }

        // GET  (Listar todos)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return Ok(tickets); // Devuelve HTTP 200
        }

        // GET 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            
            if (ticket == null)
            {
                return NotFound(new { Mensaje = $"El ticket con ID {id} no existe." }); // Devuelve HTTP 404
            }

            return Ok(ticket);
        }

        // POST 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ticket nuevoTicket)
        {
             
            if (string.IsNullOrWhiteSpace(nuevoTicket.Titulo))
            {
                return BadRequest(new { Error = "El título no puede estar vacío." }); // Devuelve HTTP 400
            }

            if (string.IsNullOrWhiteSpace(nuevoTicket.Solicitante))
            {
                return BadRequest(new { Error = "El solicitante no puede estar vacío." }); // Devuelve HTTP 400
            }

            
            nuevoTicket.FechaRegistro = DateTime.Now;
            if (string.IsNullOrWhiteSpace(nuevoTicket.Estado))
            {
                nuevoTicket.Estado = "Abierto";
            }

            _context.Tickets.Add(nuevoTicket);
            await _context.SaveChangesAsync();

            // Devuelve HTTP 201  
            return CreatedAtAction(nameof(GetById), new { id = nuevoTicket.Id }, nuevoTicket);
        }

        //  PUT: 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Ticket ticketActualizado)
        {
            if (id != ticketActualizado.Id)
            {
                return BadRequest(new { Error = "El ID de la URL no coincide con el ID del ticket." });
            }

            // ticket ha sido modificado
            _context.Entry(ticketActualizado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tickets.Any(e => e.Id == id))
                {
                    return NotFound(new { Mensaje = $"El ticket con ID {id} no existe." });
                }
                else
                {
                    throw; // Si es otro tipo de error de base de datos
                }
            }

            return NoContent(); // Devuelve HTTP 204 
        }

        // DELETE: 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound(new { Mensaje = $"El ticket con ID {id} no existe." });
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}