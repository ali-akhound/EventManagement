using EventManagement.Server.DTOs;
using EventManagement.Server.Models;

namespace EventManagement.Server.Services
{
    public interface ITicketSaleService
    {
        Task<List<TicketSaleDto>> GetTicketSaleAsync(string eventId);
        Task<List<EventDto>> GetTop5EventsByRevenueAsync();
        Task<List<EventDto>> GetTop5EventsBySalesCountAsync();
    }
}
