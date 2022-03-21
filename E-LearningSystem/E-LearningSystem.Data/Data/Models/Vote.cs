namespace E_LearningSystem.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Data.Models;
    
    public class Vote : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int TrainerId { get; set; }
    }
}
