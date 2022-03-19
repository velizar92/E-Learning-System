namespace E_LearningSystem.Web.Models.Comment
{
    using System.ComponentModel.DataAnnotations;
    public class CommentViewModel
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public int LectureId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string ProfileImageUrl { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }






    }
}
