using DistributedCaching.Models;

namespace DistributedCaching.Services
{
    public interface IProductService
    {
        Task<Product> Add(ProductCreationDto creationDto);
        Task<Product> Get(Guid id);
        Task<List<Product>> GetAll();
    }
}
