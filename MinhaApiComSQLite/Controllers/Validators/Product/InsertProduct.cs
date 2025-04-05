using Flunt.Notifications;
using Flunt.Validations;

namespace MinhaApiComSQLite.Controllers.Command.Product
{
    public class InsertProduct : Notifiable<Notification>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public int CategoryId { get; set; } = 0;

        public void Validate()
        {
            AddNotifications(new Contract<InsertProduct>().Requires()
                .IsGreaterOrEqualsThan(Name, 3, "Nome", "Nome deve conter no minímo 3 caracteres.")
                .IsLowerOrEqualsThan(Name, 100, "Nome", "Nome deve conter no máximo 100 caracteres.")
                .IsLowerOrEqualsThan(Description, 500, "Descrição", "Descrição deve conter no máximo 500 caracteres.")
                .IsGreaterThan(Price, 0, "Preço", "Produto deve haver um preço.")
                .IsGreaterThan(CategoryId, 0, "Categoria", "Selecione uma categoria."));
        }
    }
}
