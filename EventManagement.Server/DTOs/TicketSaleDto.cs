namespace EventManagement.Server.DTOs
{
    public class TicketSaleDto
    {
        public virtual string Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual DateTime PurchaseDate { get; set; }
        public virtual int PriceInCents { get; set; }
        public virtual string EventId { get; set; }
    }
}
