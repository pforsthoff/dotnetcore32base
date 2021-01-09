using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore32Base.Data.Models;
using DotNetCore32Base.Data.Repositories;

namespace DotNetCore32Base.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllCustomersAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetCustomerByIdAsync(id);
        }

        public async Task<Customer> AddCustomerAsync(Customer newCustomer)
        {
            return await _customerRepository.AddAsync(newCustomer);
        }
    }
}