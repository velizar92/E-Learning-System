﻿namespace E_LearningSystem.Web.Models.Course
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Data.Models;
    using E_LearningSystem.Services.Services.Lectures.Models;
    
    public class CourseDetailsViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public int? AssignedStudents { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public Trainer Trainer { get; set; }

        public bool IsBuyed { get; set; }

        public IEnumerable<LectureServiceModel> Lectures { get; set; }

    }
}
