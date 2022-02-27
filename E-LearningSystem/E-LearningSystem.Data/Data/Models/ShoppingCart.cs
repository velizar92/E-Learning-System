namespace E_LearningSystem.Data.Models
{
    using E_LearningSystem.Data.Data.Models;
    using System.ComponentModel.DataAnnotations;
    
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart()
        {
            this.Courses = new HashSet<Course>();
        }

        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
