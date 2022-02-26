namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Data.Data;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ELearningSystemDbContext dbContext;


        public ShoppingCartService(ELearningSystemDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }


        public Task<int> AddCourseToCart(int _shoppingCartId, int _courseId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BuyCourse(int _courseId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourseFromCart(int _shoppingCartId, int _courseId)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCartDetailsServiceModel> GetCartDetails(int _shoppingCartId)
        {
            throw new NotImplementedException();
        }
    }
}
