namespace EmpowerID.Application.RequestModels
{
    public class ProductSearchRequest
    {
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Description { get; set; }
        public DateTime? DateAddedAtStart { get; set; }
        public DateTime? DateAddedAtEnd { get; set; }
        public int Take { get; set; } = 20;
        public int Skip { get; set; } = 0;
    }
}