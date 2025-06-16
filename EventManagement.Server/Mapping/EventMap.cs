using EventManagement.Server.Models;
using FluentNHibernate.Mapping;

namespace EventManagement.Server.Mapping
{
    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Table("Events");
            Id(e =>e.Id);
            Map(e => e.Name);
            Map(e => e.StartsOn);
            Map(e => e.EndsOn);
            Map(e => e.Location);
            HasMany(e => e.TicketSales)
                .KeyColumn("EventId")
                .Inverse()
                .Cascade.All();

        }
    }
}
