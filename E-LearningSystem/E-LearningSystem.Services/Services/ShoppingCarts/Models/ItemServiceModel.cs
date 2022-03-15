namespace E_LearningSystem.Services.Services.ShoppingCarts.Models
{
    using E_LearningSystem.Services.Services.Courses.Models;

    public class ItemServiceModel
    {
        public CourseServiceModel Course { get; set; }
        public int Quantity { get; set; }
    }
}
