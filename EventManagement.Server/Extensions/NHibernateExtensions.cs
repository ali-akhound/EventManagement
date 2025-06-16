using EventManagement.Server.Mapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace EventManagement.Server.Extensions
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string Connectionstring)
        {
            var basePath = AppContext.BaseDirectory;
            var dbPath = Path.Combine(basePath, Connectionstring);
            var connectionString = $"Data Source={dbPath};";
            var session= Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                      .UsingFile(Connectionstring)
                      .ShowSql()
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EventMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TicketSaleMap>())
                .BuildSessionFactory();
            services.AddSingleton(session);
            services.AddScoped(factory => session.OpenSession());
            return services;
        }
    }
}
