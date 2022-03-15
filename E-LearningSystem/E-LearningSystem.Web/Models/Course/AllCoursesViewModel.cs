using E_LearningSystem.Services.Services.Courses.Models;

namespace E_LearningSystem.Web.Models.Course
{
    public class AllCoursesViewModel
    {
        public string ShoppingCartId { get; set; }

        public IEnumerable<AllCoursesServiceModel> AllCoursesServiceModel { get; set; }
    }
}
