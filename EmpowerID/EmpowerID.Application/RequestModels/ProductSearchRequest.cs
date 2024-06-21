namespace EmpowerID.Application.RequestModels
{
    public class ProductSearchRequest: Pagination
    {
        public string? SearchText { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? DateAddedAtStart { get; set; }
        public DateTime? DateAddedAtEnd { get; set; }
    }
}