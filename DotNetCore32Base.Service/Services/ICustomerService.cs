using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore32Base.Data.Models;

namespace DotNetCore32Base.Service.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(Customer newCustomer);
    }
}