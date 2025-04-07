using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Data.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<int> GetTotalProducts();
        Task<decimal> GetAveragePrice();
        Task<decimal> GetTotalValue();
        Task<int> GetTotalCategories();
        Task<List<SummaryDTO>> GetProductsPerCategory();
    }
}
