using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpowerID.Domain.Entities
{
    public class Product
    {
        [Key]
        [Required]
        public int Product_id { get; set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; private set; } = 00m;
       public int Category_Id { get; private set; }
       [ForeignKey(nameof(Category_Id))]
        public virtual Category Category { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
       
        public Product(string name, string description, string imageUrl,decimal price)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
        }
    }
}