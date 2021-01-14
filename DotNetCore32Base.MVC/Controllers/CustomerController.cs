using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore32Base.Data.Models;
using DotNetCore32Base.Service.Services;

namespace DotNetCore32Base.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer()
        {
            var customer = new Customer
            {
                Age = 30,
                FirstName = "Wolfgang",
                LastName = "Ofner"
            };

            return await _customerService.AddCustomerAsync(customer);
        }
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            return await _customerService.GetAllCustomersAsync();
        }
        [HttpGet]
        public async Task<ActionResult<Customer>> GetCustomerById()
        {
            return await _customerService.GetCustomerByIdAsync(1);
        }
    }
}