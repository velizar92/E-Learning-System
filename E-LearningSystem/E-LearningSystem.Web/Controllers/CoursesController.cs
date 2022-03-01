﻿namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using E_LearningSystem.Services.Services;
    using E_LearningSystem.Web.Models.Course;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;

    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        private readonly UserManager<User> userManagerService;

        public CoursesController(ICourseService _courseService, UserManager<User> _userManager)
        {
            this.courseService = _courseService;
            this.userManagerService = _userManager;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateCourse()
        {       
            return View(new CourseFormModel
            {
                Categories = await this.courseService.GetAllCourseCategories()
            });
        }


        [HttpPost]
        [Authorize]
        //Will be permitted only for Admin and Trainer roles
        public async Task<IActionResult> CreateCourse(CourseFormModel _courseModel, IFormFile _pictureFile)
        {           
            string userId = User.Id();
            ModelState.Remove("Categories");

            if (!this.courseService.CheckIfCourseCategoryExists(_courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(_courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                _courseModel.Categories = await this.courseService.GetAllCourseCategories();

                return View(_courseModel);
            }

            int courseId = await this.courseService.CreateCourse(
                                  userId,
                                 _courseModel.Name,
                                 _courseModel.Description,
                                 _courseModel.CategoryId,
                                 _pictureFile);


            return RedirectToAction(nameof(MyCourses));
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditCourse(int _id, IFormFile _pictureFile)
        {
            var course = await this.courseService.GetCourseById(_id);

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
        public async Task<IActionResult> EditCourse(int _courseId, CourseFormModel _courseModel, IFormFile _pictureFile)
        {           
            if (!this.courseService.CheckIfCourseCategoryExists(_courseModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(_courseModel.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                _courseModel.Categories = await courseService.GetAllCourseCategories();

                return View(_courseModel);
            }

            var course = courseService.EditCourse(
                                _courseId,
                                _courseModel.Name,
                                _courseModel.Description,
                                _courseModel.CategoryId,
                                _pictureFile);


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
        public async Task<IActionResult> Details(int _id)
        {
            var courseDetails = await courseService.GetCourseDetails(_id);

            return View(courseDetails);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteCourse(int _id)
        {
            var result = await courseService.DeleteCourse(_id);

            if (result == false)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(AllCourses));
        }
    }
}
