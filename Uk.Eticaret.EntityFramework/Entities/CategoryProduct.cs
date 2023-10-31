using Uk.Eticaret.EntityFramework.Entities.Abstract;

namespace Uk.Eticaret.EntityFramework.Entities
{
    public class CategoryProduct : BaseEntity
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}