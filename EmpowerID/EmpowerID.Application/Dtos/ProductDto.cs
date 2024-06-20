namespace EmpowerID.Application.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; } = 00m;
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
    }
}
