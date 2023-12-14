using Uk.Eticaret.Persistence.Entities.Abstract;

namespace Uk.Eticaret.Persistence.Entities
{
    public class Setting : BaseEntity
    {
        public int? UserId { get; set; }
        public User? User { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}