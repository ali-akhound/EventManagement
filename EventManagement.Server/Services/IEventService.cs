using EventManagement.Server.DTOs;

namespace EventManagement.Server.Services
{
    public interface IEventService
    {
        /// <summary>
        /// Retrieves a list of upcoming events within the specified number of days.
        /// </summary>
        /// <param name="days">The number of days from today to look for upcoming events.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of upcoming events.</returns>
        Task<List<EventDto>> GetUpcomingEventsAsync(int days);
    }
}
