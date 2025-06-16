using EventManagement.Server.Mapping;
using EventManagement.Server.Models;
using EventManagement.Server.Repositories;
using EventManagement.Server.Services;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework.Internal.Execution;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Test
{
    [TestFixture]
    public class EventServiceTests 
    {
        private ISessionFactory _sessionFactory;
        private ISession _session;
        private ITransaction _transaction;
        private SQLiteConnection _connection;

        private EventRepo _EventRepo;
        private TicketSaleRepo _TicketSaleRepo;
        private EventService _EventService;

        [SetUp]
        public void Setup()
        {
            _connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
            _connection.Open();

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

            _EventRepo = new EventRepo(_session);
            _TicketSaleRepo = new TicketSaleRepo(_session);
            _EventService = new EventService(_EventRepo);
            SeedTestData();
        }
        private void SeedTestData()
        {
            var event1 = new Server.Models.Event { Id = "1", Name = "Concert A", StartsOn = DateTime.UtcNow.AddDays(8), EndsOn = DateTime.UtcNow.AddHours(2), Location = "USA" };
            var event2 = new Server.Models.Event { Id = "2", Name = "Concert B", StartsOn = DateTime.UtcNow, EndsOn = DateTime.UtcNow.AddHours(61), Location = "UK" };

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
        public async Task GetUpcomingEventsAsync_ShouldReturnEventsWithinSpecifiedDays()
        {
            // Arrange
            int days = 10;
            // Act
            var result = await _EventService.GetUpcomingEventsAsync(days);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            //Assert.IsTrue(result.Any(e => e.Id == "1"));
            //Assert.IsTrue(result.Any(e => e.Id == "2"));
        }


        [Test]
        public async Task GetTicketSalesAsync_ShouldReturnSalesForEvent()
        {
            var result = await _TicketSaleRepo.GetTicketSalesAsync("1");
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetTop5EventsByRevenueAsync_ShouldReturnTopEvents()
        {
            var result = await _TicketSaleRepo.GetTop5EventsByRevenueAsync();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id); // Event 1 has more revenue
        }

        [Test]
        public async Task GetTop5EventsBySalesCountAsync_ShouldReturnTopEvents()
        {
            var result = await _TicketSaleRepo.GetTop5EventsBySalesCountAsync();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id); // Event 1 has more sales
        }

    }
}