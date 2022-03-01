﻿namespace E_LearningSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.User;

    public class User : IdentityUser
    {
        public User()
        {
            this.Courses = new HashSet<Course>();
            this.Comments = new HashSet<Comment>();
        }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }

        public ICollection<Course> Courses { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
