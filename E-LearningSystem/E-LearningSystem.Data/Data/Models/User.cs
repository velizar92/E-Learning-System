namespace E_LearningSystem.Data.Models
{
    using E_LearningSystem.Data.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.User;

    public class User : IdentityUser
    {
        public User()
        {
            this.CourseUsers = new HashSet<CourseUser>();
            this.Comments = new HashSet<Comment>();
            this.Votes = new HashSet<Vote>();
        }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }

        public ICollection<CourseUser> CourseUsers { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Vote> Votes { get; set; }
     
    }
}
