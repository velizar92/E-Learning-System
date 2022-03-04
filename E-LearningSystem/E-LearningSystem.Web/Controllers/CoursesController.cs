﻿namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;

    using static E_LearningSystem.Infrastructure.IdentityConstants;

    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly UserManager<User> userManagerService;

        public CoursesController(ICourseService courseService, UserManager<User> userManager)
        {
            this.courseService = courseService;
            this.userManagerService = userManager;
        }


        [HttpGet]
        [Authorize(Roles = AdminRole)]
        [Authorize(Roles = TrainerRole)]
        public async Task<IActionResult> CreateCourse()
        {       
            return View(new CourseFormModel
            {
                Categories = await this.courseService.GetAllCourseCategories()
            });
        }


        [HttpPost]
        [Authorize(Roles = AdminRole)]
        [Authorize(Roles = TrainerRole)]
        public async Task<IActionResult> CreateCourse(CourseFormModel courseModel, IFormFile pictureFile)
        {           
            string userId = User.Id();
            ModelState.Remove("Categories");

            if (!this.courseService.CheckIfCourseCategoryExists(courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                courseModel.Categories = await this.courseService.GetAllCourseCategories();

                return View(courseModel);
            }

            int courseId = await this.courseService.CreateCourse(
                                 userId,
                                 courseModel.Name,
                                 courseModel.Description,
                                 courseModel.Price,
                                 courseModel.CategoryId,                               
                                 pictureFile);


            return RedirectToAction(nameof(MyCourses));
        }


        [HttpGet]
        [Authorize]
        [Authorize(Roles = AdminRole)]
        [Authorize(Roles = TrainerRole)]
        public async Task<IActionResult> EditCourse(int id, IFormFile pictureFile)
        {
            var course = await this.courseService.GetCourseById(id);

            if(course == null)
            {
                return NotFound();
            }

            CourseFormModel courseFormModel = new CourseFormModel
            {
                Name = course.Name,
                Description = course.Description,
                CategoryId = course.CategoryId,              
            };

            return View(courseFormModel);
        }


        [HttpPost]
        [Authorize]
        [Authorize(Roles = AdminRole)]
        [Authorize(Roles = TrainerRole)]
        public async Task<IActionResult> EditCourse(int id, CourseFormModel courseModel, IFormFile pictureFile)
        {           
            if (!this.courseService.CheckIfCourseCategoryExists(courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                courseModel.Categories = await courseService.GetAllCourseCategories();

                return View(courseModel);
            }

            var course = courseService.EditCourse(
                                id,
                                courseModel.Name,
                                courseModel.Description,
                                courseModel.CategoryId,
                                pictureFile);


            return RedirectToAction(nameof(MyCourses));
        }



        public async Task<IActionResult> AllCourses()
        {
            var courses = await courseService.GetAllCourses();

            return View(courses);
        }


        [Authorize]
        public async Task<IActionResult> MyCourses()
        {
            string userId = User.Id();
            var myCourses = await courseService.GetMyCourses(userId);

            return View(myCourses);
        }


        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var courseDetails = await courseService.GetCourseDetails(id);

            return View(courseDetails);
        }

      
        [Authorize]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await courseService.DeleteCourse(id);

            if (result == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(AllCourses));
        }
    }
}
