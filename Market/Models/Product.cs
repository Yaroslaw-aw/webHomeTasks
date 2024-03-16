using Market.Models.Base;

namespace Market.Models
{
    public class Product : BaseModel
    {
        public Guid? CategoryId { get; set; }
        public virtual List<Category> Categories { get; set; } = new List<Category>();
    }
}
