using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Data.Repositories.Interfaces;
using MinhaApiComSQLite.Helpers;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Data.Repositories
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            return await context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();
        }

        public async Task<PagedResult<ProductDTO>> GetPaged(int pageNumber, int pageSize, int? categoryId)
        {
            var query = context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();

            return new PagedResult<ProductDTO>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<Product> GetId(int id)
        {
            try
            {
                return await context.Products
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDTO> GetIdDTO(int id)
        {
            try
            {
                return await context.Products
                    .Include(p => p.Category)
                    .Select(p => new ProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name
                    })
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> Insert(Product item)
        {
            try
            {
                context.ChangeTracker.Clear();
                await context.Products.AddAsync(item);
                await context.SaveChangesAsync();

                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> Update(Product item)
        {
            try
            {
                context.ChangeTracker.Clear();
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return item;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Delete(Product item)
        {
            try
            {
                context.ChangeTracker.Clear();
                context.Products.Remove(item);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
