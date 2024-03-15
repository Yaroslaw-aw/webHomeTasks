using Market.Models;

namespace Market.DTO
{
    public class ProductDto
    {
        //public Guid? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
    }
}
