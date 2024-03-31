using MediPortApi.HttpProcessing;

namespace MediPortApi.TagProcessing
{
    public class SimplifiedTagCalculator
    {
        private readonly int _totalTags;
        private readonly IEnumerable<Tag> _tags;

        public SimplifiedTagCalculator()
        {

        }

        public SimplifiedTagCalculator(IEnumerable<Tag> tags)
        {
            var totalTags = 0;

            _tags = tags;

            foreach (var tag in _tags)
            {
                totalTags += tag.Count;
            }
            
            _totalTags = totalTags;

        }

        public IEnumerable<SimplifiedTag> GetSortedSimplifiedTags(SortOption sortOption)
        {
            var simplifiedTags = GetSimplifiedTags();

            return sortOption switch
            {
                SortOption.NameAscending => simplifiedTags.OrderBy(x => x.Name),
                SortOption.NameDescending => simplifiedTags.OrderByDescending(x => x.Name),
                SortOption.PercentageAscending => simplifiedTags.OrderBy(x => x.Percentage),
                _ => simplifiedTags.OrderByDescending(x => x.Percentage)
            };
        }

        public IList<SimplifiedTag> GetSimplifiedTags()
        {
            var simplifiedTagsList = new List<SimplifiedTag>();
            
            foreach (var tag in _tags)
            {
                var simplifiedTag = GetSimplifiedTag(tag);
                simplifiedTagsList.Add(simplifiedTag);
            }

            return simplifiedTagsList;
        }

        public SimplifiedTag GetSimplifiedTag(Tag tag)
        {
            var roundedDivisor = Math.Round(double.Parse(tag.Count.ToString()) / _totalTags, 3);
            var percentage = roundedDivisor * 100;

            return new SimplifiedTag(tag.Name, percentage);
        }
    }
}
