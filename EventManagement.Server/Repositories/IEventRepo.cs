using EventManagement.Server.Models;

namespace EventManagement.Server.Repositories
{
    public interface IEventRepo
    {
        Task<List<Event>> GetUpcomingEventsAsync(int days);
    }
}
