using Market.Models.Base;

namespace Market.Models
{
    public class ProductStorage : BaseModel
    {
        public ulong? Count { get; set; }
        public decimal? Price { get; set; }
        public Guid? ProductId { get; set; }
        //public Guid? CategoryId { get; set; }
        public Guid? StorageId { get; set; }
    }
}
