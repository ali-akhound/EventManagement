using EventManagement.Server.Mapping;
using EventManagement.Server.Models;
using EventManagement.Server.Repositories;
using EventManagement.Server.Services;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Data.SQLite;

namespace EventManagement.Tests
{
    [TestFixture]
    public class TicketSaleServiceTests
    {
        private ISessionFactory _sessionFactory;
        private ISession _session;
        private ITransaction _transaction;
        private SQLiteConnection _connection;
        private TicketSaleRepo _repo;
        private TicketSaleService _ticketSaleService;

        [SetUp]
        public void Setup()
        {
            // Create and open the persistent in-memory connection
            _connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
            _connection.Open();

            // Configure NHibernate with existing connection
            _sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .InMemory()
                    .ShowSql()
                    .ConnectionString("Data Source=:memory:;Version=3;New=True;")
                    .Raw("connection", _connection.ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EventMap>()) 
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TicketSaleMap>()) 
                .ExposeConfiguration(cfg =>
                {
                    new SchemaExport(cfg).Execute(false, true, false, _connection, null);
                })
                .BuildSessionFactory();

            _session = _sessionFactory.OpenSession(_connection);
            _transaction = _session.BeginTransaction();

            _repo = new TicketSaleRepo(_session);
            _ticketSaleService= new TicketSaleService(_repo);

            SeedTestData();
        }

        private void SeedTestData()
        {
            var event1 = new Event { Id = "1", Name = "Concert A", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(2), Location = "USA" };
            var event2 = new Event { Id = "2", Name = "Concert B", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(2), Location = "UK" };

            _session.Save(event1);
            _session.Save(event2);

            _session.Save(new TicketSale { Id = "a", UserId = "u1", Event = event1, PriceInCents = 2000, PurchaseDate = DateTime.UtcNow });
            _session.Save(new TicketSale { Id = "b", UserId = "u2", Event = event1, PriceInCents = 3000, PurchaseDate = DateTime.UtcNow });
            _session.Save(new TicketSale { Id = "c", UserId = "u3", Event = event2, PriceInCents = 1500, PurchaseDate = DateTime.UtcNow });

            _session.Flush();
        }

        [TearDown]
        public void TearDown()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
            _session?.Dispose();
            _sessionFactory?.Dispose();
            _connection?.Dispose(); // Ensure _connection is disposed
        }

        [Test]
        public async Task GetTicketSalesAsync_ShouldReturnSalesForEvent()
        {
            var result = await _repo.GetTicketSalesAsync("1");
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetTop5EventsByRevenueAsync_ShouldReturnTopEvents()
        {
            var result = await _repo.GetTop5EventsByRevenueAsync();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id); // Event 1 has more revenue
        }

        [Test]
        public async Task GetTop5EventsBySalesCountAsync_ShouldReturnTopEvents()
        {
            var result = await _repo.GetTop5EventsBySalesCountAsync();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id); // Event 1 has more sales
        }
    }
}
