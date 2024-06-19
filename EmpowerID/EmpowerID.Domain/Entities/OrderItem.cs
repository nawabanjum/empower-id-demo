namespace EmpowerID.Domain.Entities
{
    public class OrderItem
    {
        public int ProductId { get; private set; }
        public int OrderId { get; private set; }

        public OrderItem(int productId, int orderId)
        {
            ProductId = productId;
            OrderId = orderId;
        }
    }
}
