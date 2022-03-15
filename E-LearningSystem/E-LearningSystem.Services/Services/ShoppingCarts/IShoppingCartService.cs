namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.ShoppingCarts.Models;

    public interface IShoppingCartService
    {
      
        Task<bool> BuyCourses(List<ItemServiceModel> cartItems, User user);
   
    }
}
