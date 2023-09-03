using Microsoft.AspNetCore.Mvc.ViewEngines;
using MongoDB.Bson;
using MongoDB.Driver;
using trackingAPI.Database;
using trackingAPI.Models;
using trackingAPI.Repositories;

namespace trackingAPI.Services
{
    public class CustomerHandler : ICustomers 
    {
        private readonly DBContext _mongo;
       
        public CustomerHandler(DBContext customerCollection)
        {
            _mongo = customerCollection;
        }

        public async Task<ResultList<tbCustomer>> GetCustomers()
        {
            var result = new ResultList<tbCustomer>();
            try
            {
                var get = await _mongo.customersCollection.Find(_ => true).ToListAsync();
                result.Data = get;
                return result;

            }catch(Exception ex)
            {
                result.IsCompleted = false;
                result.Message = ex.Message;
                return result;
            }
        }
        public async Task<ResultBool> GenerateMockCustomers()
        {
            var users = new List<tbCustomer>();
            for (int i = 0; i < 5; i++)
            {
                var user = new tbCustomer
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = $"192.168.0.{i + 1}",
                    GetStart = DateTime.UtcNow.AddDays(-i),
                    FinishTime = DateTime.UtcNow.AddHours(i),
                    Time = "00:00:00"
                };

                users.Add(user);
            }
            try
            {
                await _mongo.customersCollection.InsertManyAsync(users);
                return new ResultBool();
            }
            catch (Exception ex)
            {
                return new ResultBool
                {
                    IsCompleted = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResultObject<tbCustomer>> AddCustomer(tbCustomer customer)
        {
            var result = new ResultObject<tbCustomer>();
            try
            {
                await _mongo.customersCollection.InsertOneAsync(customer);
                var retrievedCustomer = await _mongo.customersCollection.Find(x => x.Id == customer.Id).FirstOrDefaultAsync();
                result.Data = retrievedCustomer;
                return result;
            }
            catch (Exception ex)
            {
                return new ResultObject<tbCustomer>
                {
                    IsCompleted = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResultObject<tbCustomer>> UpdateCustomer(string Id)
        {
            var result = new ResultObject<tbCustomer>();
            try
            {
                var customer = await _mongo.customersCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
                customer.FinishTime = DateTime.UtcNow;
                TimeSpan timeDifference = customer.FinishTime - customer.GetStart;
                int minutes = (int)timeDifference.TotalMinutes;
                int seconds = (int)timeDifference.TotalSeconds % 60;
                int hours = (int)timeDifference.TotalHours;
                customer.Time = "" +hours.ToString("00") + ':' + minutes.ToString("00") + ":" + seconds.ToString("00");
                await _mongo.customersCollection.ReplaceOneAsync(x => x.Id == customer.Id , customer);
                result.Data = customer;
                return result;
            }catch(Exception ex)
            {
                result.IsCompleted = false;
                result.Message = ex.Message;
                return result;
            }

        }
    }
}
