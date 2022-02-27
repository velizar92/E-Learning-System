﻿namespace E_LearningSystem.Services.Services.Courses.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LatestCoursesServiceModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
