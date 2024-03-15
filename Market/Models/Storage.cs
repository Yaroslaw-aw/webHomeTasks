using Market.Models.Base;

namespace Market.Models
{
    public class Storage : BaseModel
    {
        public virtual ICollection<ProductStorage> ProductStorage { get; set; } = new List<ProductStorage>();
    }
}