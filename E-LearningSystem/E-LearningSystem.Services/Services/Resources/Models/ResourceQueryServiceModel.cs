namespace E_LearningSystem.Services.Services.Resources.Models
{
    public class ResourceQueryServiceModel
    {
        public int CurrentPage { get; set; }

        public int CarsPerPage { get; set; }

        public int TotalResources { get; set; }

        public IEnumerable<AllResourcesServiceModel> Resources { get; set; }
    }
}
