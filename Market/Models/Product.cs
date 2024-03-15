using Market.Models.Base;

namespace Market.Models
{
    public class Product : BaseModel
    {
        public Guid? CategoryId { get; set; }
        public Guid? StoragetId { get; set; }
        uint? Count { get; set; }
        public decimal? Price { get; set; }
        public virtual List<Storage> Storages { get; set; } = new List<Storage>();
    }
}
