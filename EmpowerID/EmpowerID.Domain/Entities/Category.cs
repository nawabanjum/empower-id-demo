using System.ComponentModel.DataAnnotations.Schema;

namespace EmpowerID.Domain.Entities
{
    public class Category(string name)
    {
        [Column("category_id")]
        public int Id { get;private set; }

        [Column("category_name")]
        public string Name { get; private set; } = name;


        public virtual ICollection<Product>? Products { get; set; }
    }
}
