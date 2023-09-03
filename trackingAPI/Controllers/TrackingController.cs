
using trackingAPI.Repositories;
using trackingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using trackingAPI.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace trackingAPI.Controllers
{
    public class TrackingController : Hub
    {
        private readonly ICustomers _customersService;

        public TrackingController(ICustomers customersService)
        {
            _customersService = customersService;
        }

        public void ConfigureEndpoints(WebApplication app)
        {
            var groups = app.MapGroup("customer");
            groups.MapGet("/getCustomer", async () =>
            {
                var customerInfo = await _customersService.GetCustomers();
                return Results.Ok(customerInfo);
            }).Produces<tbCustomer>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status404NotFound);

            groups.MapPost("/getCustomer", async () =>
            {
                var customerInfo = await _customersService.GenerateMockCustomers();
                return Results.Ok(customerInfo);
            });

            groups.MapPut("/putCustomer", async ([FromQuery] string id) =>
            {
                var customerInfo = await _customersService.UpdateCustomer(id);
                return Results.Ok(customerInfo);
            });
        }
        public async Task ReceiveMessage(string user)
        {
            var tbCustomer = JsonConvert.DeserializeObject<tbCustomer>(user);
            var model = await _customersService.AddCustomer(tbCustomer);
            await Clients.All.SendAsync("ReceiveMessage", model);
        }
        public async Task UpdateMessage(string id)
        {
            var model = await _customersService.UpdateCustomer(id);
            await Clients.All.SendAsync("UpdateMessage", model);
        }
    }
}
