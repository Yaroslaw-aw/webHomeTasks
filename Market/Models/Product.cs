using Market.Models.Base;

namespace Market.Models
{
    public class Product : BaseModel
    {
        public Guid? CategoryId { get; set; }
        public virtual List<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
        public virtual List<ProductStorage> ProductStorages { get; set; } = new List<ProductStorage>();
    }
}

