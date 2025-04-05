namespace MinhaApiComSQLite.Models
{
    public class Product : BaseModel
    {
        public Product() { }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
