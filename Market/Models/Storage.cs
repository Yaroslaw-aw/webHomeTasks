using Market.Models.Base;

namespace Market.Models
{
    public class Storage : BaseModel
    {
        public virtual List<Category> Categories { get; set; } = new List<Category>();
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }
}