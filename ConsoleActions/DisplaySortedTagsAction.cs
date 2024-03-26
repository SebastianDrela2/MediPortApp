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

        public void DisplaySortedAscendingByName()
        {
            var tagsSortedByName = _simplifiedTags.OrderBy(x => x.Name);

            foreach(var tag in tagsSortedByName)
            {
                Console.WriteLine($"Name: {tag.Name}, Percentage: {tag.Percentage}");
            }
        }

        public void DisplaySortedDescendingByName()
        {
            var tagsSortedByName = _simplifiedTags.OrderByDescending(x => x.Name);

            foreach (var tag in tagsSortedByName)
            {
                Console.WriteLine($"Name: {tag.Name}, Percentage: {tag.Percentage}");
            }
        }

        public void DisplaySortedAscendingPercentage()
        {
            var tagsSortedByName = _simplifiedTags.OrderBy(x => x.Percentage);

            foreach (var tag in tagsSortedByName)
            {
                Console.WriteLine($"Name: {tag.Name}, Percentage: {tag.Percentage}");
            }
        }

        public void DisplaySortedDescendingByPercentage()
        {
            var tagsSortedByName = _simplifiedTags.OrderByDescending(x => x.Percentage);

            foreach (var tag in tagsSortedByName)
            {
                Console.WriteLine($"Name: {tag.Name}, Percentage: {tag.Percentage}");
            }
        }
    }
}
