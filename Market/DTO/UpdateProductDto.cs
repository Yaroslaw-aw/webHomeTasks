namespace Market.DTO
{
    public class UpdateProductDto
    {
        public Guid? productId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}