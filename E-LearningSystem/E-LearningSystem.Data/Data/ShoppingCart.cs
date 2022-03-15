namespace E_LearningSystem.Data.Data
{
    using E_LearningSystem.Data.Data.Models;

    public class ShoppingCart
    {
        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
