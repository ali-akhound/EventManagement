using EventManagement.Server.Models;
using FluentNHibernate.Mapping;

namespace EventManagement.Server.Mapping
{
    public class TicketSaleMap:ClassMap<TicketSale>
    {
        public TicketSaleMap()
        {
            Table("TicketSales");
            Id(x=>x.Id);
            Map(x => x.UserId);
            Map(x => x.PurchaseDate);
            Map(x => x.PriceInCents);
            References(x=>x.Event)                
                .Column("EventId")
                .Not.Nullable()
                .Cascade.All();
        }
    }
}
