using Flunt.Notifications;
using Flunt.Validations;
using MinhaApiComSQLite.Controllers.Command.Product;

namespace MinhaApiComSQLite.Controllers.Validators.Category
{
    public class UpdateCategory : Notifiable<Notification>
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        public void Validate()
        {
            AddNotifications(new Contract<InsertProduct>().Requires()
                .IsGreaterThan(Id, 0, "Id", "Selecione uma categoria.")
                .IsGreaterOrEqualsThan(Name, 3, "Nome", "Nome deve conter no minímo 3 caracteres.")
                .IsLowerOrEqualsThan(Name, 100, "Nome", "Nome deve conter no máximo 100 caracteres."));
        }
    }
}
