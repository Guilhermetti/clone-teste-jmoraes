using MinhaApiComSQLite.Helpers;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Data.Repositories.Interfaces
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<IEnumerable<ProductDTO>> GetAll();
        Task<PagedResult<ProductDTO>> GetPaged(int pageNumber, int pageSize, int? categoryId);
        Task<ProductDTO> GetIdDTO(int id);
        Task<Product> GetId(int id);
    }
}
