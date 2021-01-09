using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore32Base.Data.Models;

namespace DotNetCore32Base.Data.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetCustomerByIdAsync(int id);

        Task<List<Customer>> GetAllCustomersAsync();
    }
}