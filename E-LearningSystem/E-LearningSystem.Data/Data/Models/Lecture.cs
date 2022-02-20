﻿namespace E_LearningSystem.Data.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.Lecture;

    public class Lecture
    {
        public Lecture()
        {
            this.Resources = new HashSet<Resource>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(LectureNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(LectureDescriptionMaxLength)]
        public string Description { get; set; }


        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; }


        public ICollection<Resource> Resources { get; set; }

    }
}