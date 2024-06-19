using System.ComponentModel.DataAnnotations;

namespace EmpowerID.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        [Required]
        public int Id { get; private set; }
        public DateTime AddedAt { get; private set; }

        public BaseEntity()
        {
                AddedAt = DateTime.UtcNow;
        }
    }
}
