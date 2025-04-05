using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Data.Repositories.Interfaces
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<IEnumerable<CategoryDTO>> GetAll();
        Task<CategoryDTO> GetIdDTO(int id);
        Task<Category> GetId(int id);
        Task<Category> GetName(string name);
    }
}
