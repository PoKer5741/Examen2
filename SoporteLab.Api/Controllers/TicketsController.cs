using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteLab.Api.Data;
using SoporteLab.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: /api/tickets (Listar todos)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return Ok(tickets); // Devuelve HTTP 200
        }

        // GET: /api/tickets/{id} (Consultar por ID)
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

        // POST: /api/tickets (Crear nuevo ticket)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ticket nuevoTicket)
        {
            // Validaciones mínimas requeridas por la rúbrica
            if (string.IsNullOrWhiteSpace(nuevoTicket.Titulo))
            {
                return BadRequest(new { Error = "El título no puede estar vacío." }); // Devuelve HTTP 400
            }

            if (string.IsNullOrWhiteSpace(nuevoTicket.Solicitante))
            {
                return BadRequest(new { Error = "El solicitante no puede estar vacío." }); // Devuelve HTTP 400
            }

            // Forzar fecha de registro del servidor y estado inicial seguro
            nuevoTicket.FechaRegistro = DateTime.Now;
            if (string.IsNullOrWhiteSpace(nuevoTicket.Estado))
            {
                nuevoTicket.Estado = "Abierto";
            }

            _context.Tickets.Add(nuevoTicket);
            await _context.SaveChangesAsync();

            // Devuelve HTTP 201 Created con la ruta para consultar el recurso creado
            return CreatedAtAction(nameof(GetById), new { id = nuevoTicket.Id }, nuevoTicket);
        }

        // NUEVO: PUT: /api/tickets/{id} (Actualizar un ticket existente)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Ticket ticketActualizado)
        {
            if (id != ticketActualizado.Id)
            {
                return BadRequest(new { Error = "El ID de la URL no coincide con el ID del ticket." });
            }

            // Le decimos a Entity Framework que este ticket ha sido modificado
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
                    throw; // Si es otro tipo de error de base de datos, lo lanzamos
                }
            }

            return NoContent(); // Devuelve HTTP 204 (Éxito, pero no hay nada nuevo que devolver)
        }
    }
}