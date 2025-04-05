namespace MinhaApiComSQLite.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<ProductSimpleDTO> Produtos { get; set; } = new();
    }
}
