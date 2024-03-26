using MediPortSOAPI.TagProcessing;

namespace MediPortSOAPI.ConsoleActions
{
    internal class DisplaySortedTagsAction
    {
        private readonly IList<SimplifiedTag> _simplifiedTags;

        public DisplaySortedTagsAction(IList<SimplifiedTag> simplifiedTags)
        {
            _simplifiedTags = simplifiedTags;
        }

        public void DisplaySortedTags(SortOption sortOption)
        {
            var simplifedTagsSorted = GetSortedSimplifiedTags(sortOption);
            
            foreach (var tag in simplifedTagsSorted)
            {
                Console.WriteLine($"Name: {tag.Name}, Percentage: {tag.Percentage}%");
            }
        }

        private IEnumerable<SimplifiedTag> GetSortedSimplifiedTags(SortOption sortOption)
        {
            return sortOption switch
            {
                SortOption.NameAscending => _simplifiedTags.OrderBy(x => x.Name),
                SortOption.NameDescending => _simplifiedTags.OrderByDescending(x => x.Name),
                SortOption.PercentageAscending => _simplifiedTags.OrderBy(x => x.Percentage),
                _ => _simplifiedTags.OrderByDescending(x => x.Percentage)
            };
        }
    }
}
