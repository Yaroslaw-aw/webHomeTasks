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

// 1157c6c2-b067-4c58-86cb-114243d9200d   - категория
// 6e155182-c5eb-48fd-9554-8851ad28d027   - склад
// {
//   "categoryId": "1157c6c2-b067-4c58-86cb-114243d9200d",
//     "storagetId": "6e155182-c5eb-48fd-9554-8851ad28d027",
//     "count": 85,
//     "price": 35.89,
//     "storages": [],
//     "id": "29b264f9-fa8a-41d0-8ad1-71651e4af3e6",
//     "name": "Coca-cola",
//     "description": "2 L"
// }