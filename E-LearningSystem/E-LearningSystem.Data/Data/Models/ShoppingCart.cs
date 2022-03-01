namespace E_LearningSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations; 
     
    public class ShoppingCart : BaseEntity
    {
        public ShoppingCart()
        {
            this.Courses = new HashSet<Course>();
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
