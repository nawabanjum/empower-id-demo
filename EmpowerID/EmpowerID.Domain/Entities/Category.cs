using System.ComponentModel.DataAnnotations.Schema;

namespace EmpowerID.Domain.Entities
{
    public class Category
    {
        [Column("category_id")]
        public int Id { get;private set; }

        [Column("category_name")]
        public string Name { get; private set; } = string.Empty;


        public virtual ICollection<Product> Products { get; set; }

        public Category(string name)
        {
            Name = name;
        }
    }
}
