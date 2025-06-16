using EventManagement.Server.DTOs;
using EventManagement.Server.Models;
using NHibernate.Linq;

namespace EventManagement.Server.Repositories
{
    public class EventRepo : IEventRepo
    {
        private readonly NHibernate.ISession _session;

        public EventRepo(NHibernate.ISession session)
        {
            _session = session;
        }
        public async Task<List<Event>> GetUpcomingEventsAsync(int days)
        {
           return await _session.Query<Event>()               
                .Where(e =>
                    e.StartsOn.Date >= DateTime.UtcNow.Date
                    && e.StartsOn.Date <= DateTime.UtcNow.Date.AddDays(days))
                .ToListAsync();
        }
    }
}
