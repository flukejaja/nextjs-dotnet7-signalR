using trackingAPI.Controllers;
using trackingAPI.Database;
using trackingAPI.Models;
using trackingAPI.Repositories;
using trackingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DBContext>();
builder.Services.AddSingleton<ICustomers, CustomerHandler>();
builder.Services.AddSignalR();
builder.Services.Configure<TrackingDatabaseSettings>(
builder.Configuration.GetSection("TrackingDatabase"));
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowAnyOrigin()
    );
app.UseHttpsRedirection();
app.MapHub<TrackingController>("/timer");
app.UseAuthorization();
var trackingController = new TrackingController(app.Services.GetRequiredService<ICustomers>());
trackingController.ConfigureEndpoints(app);


app.Map("/exception", ()
    => { throw new InvalidOperationException("Sample Exception"); });
app.MapControllers();


app.Run();
