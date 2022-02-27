namespace E_LearningSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    using static DataConstants.Resource;

    public class Resource : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ResourceNameMaxLength)]
        public string Name { get; set; }


        [ForeignKey(nameof(ResourceType))]
        public int ResourceTypeId { get; set; }
        public ResourceType ResourceType { get; set; }


        [ForeignKey(nameof(Lecture))]
        public int LectureId { get; set; }
        public Lecture Lecture { get; set; }
    }
}
