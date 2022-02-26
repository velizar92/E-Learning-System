namespace E_LearningSystem.Services.Services
{
    public interface IShoppingCartService
    {
        Task<int> AddCourseToCart(int _shoppingCartId, int _courseId);

        Task<bool> DeleteCourseFromCart(int _shoppingCartId, int _courseId);
      
        Task<ShoppingCartDetailsServiceModel> GetCartDetails(int _shoppingCartId);

        Task<bool> BuyCourse(int _courseId);
    }
}
