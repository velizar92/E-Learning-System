namespace E_LearningSystem.Data.Data.Models
{
    using E_LearningSystem.Data.Models;

    public class ShoppingCartItem : BaseEntity
    {
        public int Id { get; set; }

        public Course Course { get; set; }

        public int Quantity { get; set; }

        public string ShoppingCartId { get; set; }

        public string UserId { get; set; }

    }
}
