using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Data.Repositories.Interfaces;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Data.Repositories
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            return await context.Categories
                .Include(c => c.Products)
                    .Select(c => new CategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Products = c.Products.Select(p => new ProductSimpleDTO
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price
                        }).ToList()
                    })
                .ToListAsync();
        }

        public async Task<CategoryDTO> GetIdDTO(int id)
        {
            try
            {
                var category = await context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return null!;

                return new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Products = category.Products.Select(p => new ProductSimpleDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price
                    }).ToList()
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> GetId(int id)
        {
            try
            {
                return await context.Categories
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> GetName(string name)
        {
            try
            {
                return await context.Categories
                    .FirstOrDefaultAsync(c => c.Name == name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> Insert(Category item)
        {
            try
            {
                context.ChangeTracker.Clear();
                await context.Categories.AddAsync(item);
                await context.SaveChangesAsync();

                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> Update(Category item)
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

        public async Task<bool> Delete(Category item)
        {
            try
            {
                context.ChangeTracker.Clear();
                context.Categories.Remove(item);
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
