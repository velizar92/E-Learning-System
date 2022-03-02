namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Models;

    public interface IShoppingCartService
    {
        Task<bool> AddCourseToCart(string shoppingCartId, int courseId);

        Task<bool> DeleteCourseFromCart(string shoppingCartId, int courseId);
      
        Task<ShoppingCartDetailsServiceModel> GetCartDetails(string shoppingCartId);

        Task<bool> BuyCourses(string shoppingCartId);

        Task<ShoppingCart> GetCartById(string shoppingCartId);
    }
}
