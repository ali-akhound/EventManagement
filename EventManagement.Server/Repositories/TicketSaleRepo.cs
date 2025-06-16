using EventManagement.Server.Models;
using NHibernate.Linq;

namespace EventManagement.Server.Repositories
{
    public class TicketSaleRepo : ITicketSaleRepo
    {
        private readonly NHibernate.ISession _session;

        public TicketSaleRepo(NHibernate.ISession session)
        {
            _session = session;
        }
        public async Task<List<TicketSale>> GetTicketSalesAsync(string eventId)
        {
            return await _session.Query<TicketSale>()
                .Where(ts => ts.Event.Id == eventId)
            .ToListAsync();
        }

        public async Task<List<Event>> GetTop5EventsByRevenueAsync()
        {
            var events = await _session.Query<TicketSale>()
                .GroupBy(e => new { e.Event })
                .Select(g => new
                {
                    g.Key.Event,
                    SumTicketSales = g.Sum(ts => ts.PriceInCents)
                })
                .OrderByDescending(e => e.SumTicketSales)
                .Take(5)                
                .ToListAsync();
            return events.Select(e => e.Event).ToList();
        }

        public async Task<List<Event>> GetTop5EventsBySalesCountAsync()
        {
            var events = await _session.Query<TicketSale>()
                .GroupBy(e => new { e.Event })
                .Select(t => new
                {
                    t.Key.Event,
                    Count = t.Count()
                })
                .OrderByDescending(t => t.Count)
                .Take(5)
                .ToListAsync();
            return events.Select(e => e.Event).ToList();
        }
    }
}
