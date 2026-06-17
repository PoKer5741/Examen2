using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using SoporteLab.Web.Models;
using System;

namespace SoporteLab.Web.Services
{
    public class TicketService
    {
        private readonly HttpClient _httpClient;

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Ticket>?> GetTicketsAsync()
        {
            try { return await _httpClient.GetFromJsonAsync<List<Ticket>>("api/tickets"); }
            catch { return new List<Ticket>(); }
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            try { return await _httpClient.GetFromJsonAsync<Ticket>($"api/tickets/{id}"); }
            catch { return null; }
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            try {
                var response = await _httpClient.PostAsJsonAsync("api/tickets", ticket);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateTicketAsync(Ticket ticketActualizado)
        {
            try 
            {
                var response = await _httpClient.PutAsJsonAsync($"api/tickets/{ticketActualizado.Id}", ticketActualizado);
                
                // SI LA API RECHAZA EL TICKET, ESTO NOS DIRÁ POR QUÉ EN LA TERMINAL
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[ERROR API] Código de estado: {response.StatusCode}");
                    var detalleError = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[DETALLE API] Respuesta del servidor: {detalleError}");
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR CRÍTICO] Fallo de red al conectar con la API: {ex.Message}");
                return false;
            }
        }
    }
}