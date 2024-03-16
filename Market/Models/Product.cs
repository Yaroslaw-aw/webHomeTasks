using Market.Models.Base;

namespace Market.Models
{
    public class Product : BaseModel
    {
        public Guid? CategoryId { get; set; }
        //public Guid? StorageId { get; set; }
        //public ulong? Count { get; set; }
        //public decimal? Price { get; set; }
        public virtual List<Category> Categories { get; set; } = new List<Category>();
        //public virtual List<Storage> Storages { get; set; } = new List<Storage>();
    }
}
