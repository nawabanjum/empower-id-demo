using System.ComponentModel.DataAnnotations.Schema;

namespace EmpowerID.Domain.Entities
{
    public class Order(string customerName)
    {
        [Column("order_id")]
        public int Id { get; private set; }
        [Column("customer_name")]
        public string CustomerName { get; private set; } = customerName;
        [Column("order_date")]
        public DateTime OrderDate { get; private set; }


        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
