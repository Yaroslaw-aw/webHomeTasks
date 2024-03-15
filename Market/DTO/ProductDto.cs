using Market.Models;

namespace Market.DTO
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public uint? Count { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? StoragetId { get; set; }
    }
}
