namespace MinhaApiComSQLite.Models
{
    public class Category : BaseModel
    {
        public Category() { }

        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = null!;
    }
}
