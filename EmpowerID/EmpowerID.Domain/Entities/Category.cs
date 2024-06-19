namespace EmpowerID.Domain.Entities
{
    public class Category :BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        public Category(string name)
        {
            Name = name;
        }
    }
}
