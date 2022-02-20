namespace E_LearningSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.ResourceType;

    public class ResourceType
    {
        public ResourceType()
        {
            this.Resources = new HashSet<Resource>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ResourceTypeNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Resource> Resources { get; set; }
    }
}
