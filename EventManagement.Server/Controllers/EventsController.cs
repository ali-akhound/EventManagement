using EventManagement.Server.DTOs;
using EventManagement.Server.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ITicketSaleService _ticketSaleService;

        public EventsController(IEventService eventService,ITicketSaleService ticketSaleService)
        {
            _eventService = eventService;
            _ticketSaleService = ticketSaleService;
        }
        // GET: api/<EventsController>
        [HttpGet("{days}")]
        public async Task<List<EventDto>> Get(int days)
        {
            return await _eventService.GetUpcomingEventsAsync(days);
        }
        [HttpGet("GetTop5EventsByRevenue")]
        public async Task<List<EventDto>> GetTop5EventsByRevenue()
        {
            return await _ticketSaleService.GetTop5EventsByRevenueAsync();
        }
        [HttpGet("GetTickets/{eventID}")]
        public async Task<List<TicketSaleDto>> GetTickets(string eventID)
        {
            return await _ticketSaleService.GetTicketSaleAsync(eventID);
        }
        [HttpGet("GetTop5EventsBySalesCount")]
        public async Task<List<EventDto>> GetTop5EventsBySalesCount()
        {
            return await _ticketSaleService.GetTop5EventsBySalesCountAsync();
        }

    }
}
