using System.Threading.Tasks;
using DotNetCore32Base.Data.Models;

namespace DotNetCore32Base.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductByIdAsync(int id);
    }
}