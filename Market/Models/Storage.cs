using Market.Models.Base;

namespace Market.Models
{
    public class Storage : BaseModel
    {
        public int Count { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
        public virtual List<Category> Categories { get; set; } = new List<Category>();
    }
}