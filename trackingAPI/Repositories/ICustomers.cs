using trackingAPI.Models;

namespace trackingAPI.Repositories
{
    public interface ICustomers 
    {
        public Task<ResultList<tbCustomer>>GetCustomers();
        public Task<ResultBool> GenerateMockCustomers();
        public Task<ResultObject<tbCustomer>> AddCustomer(tbCustomer customer);
        public Task<ResultObject<tbCustomer>> UpdateCustomer(string id);
    }
}
