namespace E_LearningSystem.Web.Models.Course
{
    using E_LearningSystem.Services.Services.Courses.Models;
    public class CourseDetailsViewModel
    {
        public CourseDetailsServiceModel CourseServiceModel { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
