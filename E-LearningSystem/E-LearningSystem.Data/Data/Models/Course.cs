﻿namespace E_LearningSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using static DataConstants.Course;

    public class Course : BaseEntity
    {
        public Course()
        {
            this.Lectures = new HashSet<Lecture>();
            this.Users = new HashSet<User>();
            this.Issues = new HashSet<Issue>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CourseNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CourseDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public double Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int? AssignedStudents { get; set; }

        public string UserId { get; set; }


        [ForeignKey(nameof(CourseCategory))]
        public int CourseCategoryId { get; set; }

        public CourseCategory CourseCategory { get; set; }


        public ICollection<User> Users { get; set; }

        public ICollection<Lecture> Lectures { get; set; }

        public ICollection<Issue> Issues { get; set; }

    }
}
