using E_LearningSystem.Data.Models;

namespace E_LearningSystem.Services.Services
{
    public interface IShoppingCartService
    {
        Task<bool> AddCourseToCart(int _shoppingCartId, int _courseId);

        Task<bool> DeleteCourseFromCart(int _shoppingCartId, int _courseId);
      
        Task<ShoppingCartDetailsServiceModel> GetCartDetails(int _shoppingCartId);

        Task<bool> BuyCourses(int _shoppingCartId);

        Task<ShoppingCart> GetCartById(int _shoppingCartId);
    }
}
