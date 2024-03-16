using Market.Models.Base;

namespace Market.Models
{
    public class Product : BaseModel
    {
        public Guid? CategoryId { get; set; }
        public virtual List<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>(); // Замените на CategoryProducts
        public virtual List<ProductStorage> ProductStorages { get; set; } = new List<ProductStorage>();
        //public virtual List<Category> Categories { get; set; } = new List<Category>(); // Используйте правильное название свойства
    }
}

