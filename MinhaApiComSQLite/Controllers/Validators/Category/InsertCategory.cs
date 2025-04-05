using Flunt.Notifications;
using Flunt.Validations;
using MinhaApiComSQLite.Controllers.Command.Product;

namespace MinhaApiComSQLite.Controllers.Validators.Category
{
    public class InsertCategory : Notifiable<Notification>
    {
        public string Name { get; set; } = string.Empty;

        public void Validate()
        {
            AddNotifications(new Contract<InsertProduct>().Requires()
                .IsGreaterOrEqualsThan(Name, 3, "Nome", "Nome deve conter no minímo 3 caracteres.")
                .IsLowerOrEqualsThan(Name, 100, "Nome", "Nome deve conter no máximo 100 caracteres."));
        }
    }
}
