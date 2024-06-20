using System.ComponentModel.DataAnnotations.Schema;

namespace EmpowerID.Domain.Entities
{
    public class OrderItem
    {
        [Column("order_item_id")]
        public int OrderItemId { get; set; }

        [Column("product_id")]
        public int ProductId { get; private set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [Column("order_id")]
        public int OrderId { get; private set; }
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        public OrderItem(int productId, int orderId)
        {
            ProductId = productId;
            OrderId = orderId;
        }
    }
}
