using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Data.Repositories.Interfaces;
using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Data.Repositories
{
    public class ReportRepository(AppDbContext context) : IReportRepository
    {
        public async Task<int> GetTotalProducts()
        {
            return await context.Products.CountAsync();
        }

        public async Task<decimal> GetAveragePrice()
        {
            return await context.Products.AnyAsync()
                ? Math.Round(await context.Products.AverageAsync(p => p.Price), 2)
                : 0;
        }

        public async Task<decimal> GetTotalValue()
        {
            return await context.Products.AnyAsync()
                ? await context.Products.SumAsync(p => p.Price)
                : 0;
        }

        public async Task<int> GetTotalCategories()
        {
            return await context.Categories.CountAsync();
        }

        public async Task<List<SummaryDTO>> GetProductsPerCategory()
        {
            return await context.Categories
                .Select(c => new SummaryDTO
                {
                    Category = c.Name,
                    ProductCount = context.Products.Count(p => p.CategoryId == c.Id)
                }).ToListAsync();
        }
    }
}