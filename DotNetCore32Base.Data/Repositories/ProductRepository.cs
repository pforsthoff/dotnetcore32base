using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetCore32Base.Data.Models;

namespace DotNetCore32Base.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(RepositoryPatternDemoContext repositoryPatternDemoContext) : base(repositoryPatternDemoContext)
        {
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}