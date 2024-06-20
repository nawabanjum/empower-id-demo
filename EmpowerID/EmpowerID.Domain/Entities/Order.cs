using System.ComponentModel.DataAnnotations.Schema;

namespace EmpowerID.Domain.Entities
{
    public class Order 
    {
        [Column("order_id")]
        public int Id { get; private set; }
        [Column("customer_name")]
        public string CustomerName { get; private set; } = string.Empty;
        [Column("order_date")]
        public DateTime OrderDate { get; private set; }


        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public Order(string customerName)
        {
            CustomerName = customerName;
        }
    }
}
