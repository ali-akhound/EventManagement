namespace EventManagement.Server.DTOs
{
    public class EventDto
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime StartsOn { get; set; }
        public virtual DateTime EndsOn { get; set; }
        public virtual string Location { get; set; }
        public IList<TicketSaleDto> TicketSales { get; set; } = new List<TicketSaleDto>();
    }
}
