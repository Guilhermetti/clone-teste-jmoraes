using System.ComponentModel.DataAnnotations;

namespace MinhaApiComSQLite.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
