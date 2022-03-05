namespace E_LearningSystem.Web.Models.Resource
{
    using System.ComponentModel.DataAnnotations;
    using E_LearningSystem.Services.Services.Resources;
    using E_LearningSystem.Services.Services.Resources.Models;

    public class AllResourcesQueryModel
    {
        public const int ResourcesPerPage = 6;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public int ResourceTypeId { get; set; }

        public string ResourceType { get; set; }

        public ResourceSorting SortingCriteria { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalResources { get; set; }

        public IEnumerable<string> ResourceTypes { get; set; }

        public IEnumerable<AllResourcesServiceModel> Resources { get; set; }
    }
}
