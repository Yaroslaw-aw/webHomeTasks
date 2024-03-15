using Market.Models.Base;

namespace Market.Models
{
    public class Category : BaseModel
    {
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
