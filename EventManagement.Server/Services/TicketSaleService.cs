using EventManagement.Server.DTOs;
using EventManagement.Server.Models;
using EventManagement.Server.Repositories;

namespace EventManagement.Server.Services
{
    public class TicketSaleService : ITicketSaleService
    {
        private readonly ITicketSaleRepo _ticketSaleRepo;

        public TicketSaleService(ITicketSaleRepo ticketSaleRepo)
        {
            _ticketSaleRepo = ticketSaleRepo;
        }
        public async Task<List<TicketSaleDto>> GetTicketSaleAsync(string eventId)
        {
            var tickets = await _ticketSaleRepo.GetTicketSalesAsync(eventId);
            return tickets.Select(e => new TicketSaleDto()
            {
                Id = e.Id,
                EventId = e.Event.Id,
                PriceInCents = e.PriceInCents,
                PurchaseDate = e.PurchaseDate,
                UserId = e.UserId,
            }).ToList();
        }

        public async Task<List<EventDto>> GetTop5EventsByRevenueAsync()
        {
            var events = await _ticketSaleRepo.GetTop5EventsByRevenueAsync();
            return events.Select(e => new EventDto()
            {
                Id = e.Id,
                Name = e.Name,
                StartsOn = e.StartsOn,
                EndsOn = e.EndsOn,
                Location = e.Location
            }).ToList();
        }

        public async Task<List<EventDto>> GetTop5EventsBySalesCountAsync()
        {
            var events = await _ticketSaleRepo.GetTop5EventsBySalesCountAsync();
            return events.Select(e => new EventDto()
            {
                Id = e.Id,
                Name = e.Name,
                StartsOn = e.StartsOn,
                EndsOn = e.EndsOn,
                Location = e.Location
            }).ToList();
        }
    }
}
