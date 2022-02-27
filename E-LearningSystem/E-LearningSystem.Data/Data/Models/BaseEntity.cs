namespace E_LearningSystem.Data.Data.Models
{
    public abstract class BaseEntity
    {
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
