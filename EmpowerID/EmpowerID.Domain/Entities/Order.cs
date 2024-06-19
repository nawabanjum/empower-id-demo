namespace EmpowerID.Domain.Entities
{
    public class Order :BaseEntity
    {
        public string CustomerName { get; private set; } = string.Empty;
        
        public Order(string customerName)
        {
            CustomerName = customerName;
        }
    }
}
