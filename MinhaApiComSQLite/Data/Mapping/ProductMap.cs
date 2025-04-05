using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Data.Mapping
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CategoryId).HasColumnName("CategoriaId").HasColumnType("int");
            builder.Property(p => p.Name).HasColumnName("Nome").IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasColumnName("Descricao").HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnName("Preco").HasColumnType("decimal(10,2)");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
