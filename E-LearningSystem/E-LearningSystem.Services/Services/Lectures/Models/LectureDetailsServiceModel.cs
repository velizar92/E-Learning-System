namespace E_LearningSystem.Services.Services
{
    public class LectureDetailsServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] ResourceUrls { get; set; }
    }
}