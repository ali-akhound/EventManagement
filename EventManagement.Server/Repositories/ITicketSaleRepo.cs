using EventManagement.Server.Models;

namespace EventManagement.Server.Repositories
{
    public interface ITicketSaleRepo
    {
        Task<List<TicketSale>> GetTicketSalesAsync(string eventId);
        Task<List<Event>> GetTop5EventsBySalesCountAsync();

        Task<List<Event>> GetTop5EventsByRevenueAsync();
    }
}
