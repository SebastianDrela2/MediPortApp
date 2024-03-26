using MediPortApi.HttpProcessing;
using MediPortApi.TagProcessing;

namespace MediPortApi.ConsoleActions
{
    internal class DisplaySortedTagsAction
    {
        private readonly SimplifiedTagCalculator _simplifiedTagCalculator;

        public DisplaySortedTagsAction(SimplifiedTagCalculator simplifiedTagCalculator)
        {
            _simplifiedTagCalculator = simplifiedTagCalculator;
        }
        
        public void DisplaySortedTags(SortOption sortOption)
        {
            var sortedSimplifiedTags = _simplifiedTagCalculator.GetSortedSimplifiedTags(sortOption);

            foreach(var tag in sortedSimplifiedTags)
            {
                Console.WriteLine($"Name: {tag.Name} Percentage: {tag.Percentage}");
            }
        }
    }
}
