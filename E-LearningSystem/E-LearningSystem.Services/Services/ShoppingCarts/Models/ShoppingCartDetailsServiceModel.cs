namespace E_LearningSystem.Services.Services
{
    using E_LearningSystem.Services.Services.Courses.Models;

    public class ShoppingCartDetailsServiceModel
    {
        public int CartId { get; set; }

        public IEnumerable<CourseDetailsServiceModel> Courses { get; set; }
    }
}