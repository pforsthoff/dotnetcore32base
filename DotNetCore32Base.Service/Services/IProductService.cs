using System.Threading.Tasks;
using DotNetCore32Base.Data.Models;

namespace DotNetCore32Base.Service.Services
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(Product product);

        Task<Product> GetProductByIdAsync(int id);
    }
}