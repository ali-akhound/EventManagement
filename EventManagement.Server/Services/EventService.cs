using EventManagement.Server.DTOs;
using EventManagement.Server.Repositories;

namespace EventManagement.Server.Services
{
    public class EventService:IEventService
    {
        private readonly IEventRepo _eventRepo;
        public EventService(IEventRepo eventRepo)
        {
            _eventRepo = eventRepo;
        }
        /// <summary>
        /// Retrieves a list of upcoming events within the specified number of days.
        /// </summary>
        /// <param name="days">The number of days from today to look for upcoming events.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of upcoming events.</returns>
        public async Task<List<EventDto>> GetUpcomingEventsAsync(int days)
        {
            var events = await _eventRepo.GetUpcomingEventsAsync(days);
            return events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Location = e.Location,
                StartsOn = e.StartsOn,
                EndsOn = e.EndsOn
            }).ToList();
        }

    }
}
