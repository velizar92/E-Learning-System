namespace E_LearningSystem.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;

    using static E_LearningSystem.Infrastructure.Messages.ErrorMessages;

    public class CoursesController : AdminController
    {

        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
        }


        public async Task<IActionResult> Index()
        {
            var allCourses = await this.courseService.GetAllCourses();

            return View(allCourses);
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await this.courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }

            CourseFormModel courseFormModel = new CourseFormModel
            {
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                CategoryId = course.CategoryId,
                Categories = await courseService.GetAllCourseCategories()
            };

            return View(courseFormModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, CourseFormModel courseModel)
        {
            if (!this.courseService.CheckIfCourseCategoryExists(courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(courseModel.CategoryId), CategoryNotExists);
            }

            if (!ModelState.IsValid)
            {
                courseModel.Categories = await this.courseService.GetAllCourseCategories();

                return View(courseModel);
            }

            var (isEdited, errorMessage) = await this.courseService.EditCourse(
                                id,
                                courseModel.Name,
                                courseModel.Description,
                                courseModel.Price,
                                courseModel.CategoryId,
                                courseModel.PictureFile.FileName);

            if (isEdited == false)
            {
                return BadRequest(errorMessage);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteCourse(int id)
        {          
            var (result, errorMessage) = await courseService.DeleteCourse(id);

            if (result == false)
            {
                return BadRequest(errorMessage);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
