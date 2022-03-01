namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Models;

    public interface IShoppingCartService
    {
        Task<bool> AddCourseToCart(string _shoppingCartId, int _courseId);

        Task<bool> DeleteCourseFromCart(string _shoppingCartId, int _courseId);
      
        Task<ShoppingCartDetailsServiceModel> GetCartDetails(string _shoppingCartId);

        Task<bool> BuyCourses(string _shoppingCartId);

        Task<ShoppingCart> GetCartById(string _shoppingCartId);
    }
}
