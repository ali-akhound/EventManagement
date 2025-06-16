using EventManagement.Server.Extensions;
using EventManagement.Server.Repositories;
using EventManagement.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register NHibernate with SQLite configuration
builder.Services.AddNHibernate(builder.Configuration.GetConnectionString("SQLite"));

builder.Services.AddScoped<IEventRepo, EventRepo>();
builder.Services.AddScoped<ITicketSaleRepo, TicketSaleRepo>();
builder.Services.AddScoped<IEventService,EventService>();
builder.Services.AddScoped<ITicketSaleService, TicketSaleService>();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:65100")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();

                      });
});
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(MyAllowSpecificOrigins);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
