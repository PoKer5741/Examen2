using System.Net.Http.Json;
using SoporteLab.Web.Models;

namespace SoporteLab.Web.Services
{
    public class TicketService
    {
        private readonly HttpClient _httpClient;

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para traer la lista completa (GET)
        public async Task<List<Ticket>> GetTicketsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Ticket>>("api/tickets") ?? new List<Ticket>();
        }

        // Método para enviar un nuevo ticket a la API (POST)
        public async Task<bool> CreateTicketAsync(Ticket nuevoTicket)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tickets", nuevoTicket);
            return response.IsSuccessStatusCode; // Devuelve true si se guardó con éxito
        }
    }
}