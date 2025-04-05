using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Data.Repositories.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseModel
    {
        Task<T> Insert(T item);
        Task<T> Update(T item);
        Task<bool> Delete(T item);
    }
}
