namespace E_LearningSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.User;

    public class User : IdentityUser
    {

        public User()
        {
            this.Courses = new HashSet<Course>();
        }

        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }


        [ForeignKey(nameof(Trainer))]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }


        public ICollection<Course> Courses { get; set; }
    }
}
