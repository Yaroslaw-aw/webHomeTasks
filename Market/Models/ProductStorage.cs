using Market.Models.Base;

namespace Market.Models
{
    public class ProductStorage : BaseModel
    {
        public ulong? Count { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? StorageId { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
        public virtual List<Category> Categories { get; set; } = new List<Category>();
    }
}
