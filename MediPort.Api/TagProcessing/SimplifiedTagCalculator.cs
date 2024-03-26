using MediPortApi.HttpProcessing;

namespace MediPortApi.TagProcessing
{
    public class SimplifiedTagCalculator
    {
        private readonly int _totalTags;
        private readonly List<Tag> _tags;

        public SimplifiedTagCalculator(TagsData tagsData)
        {
            var totalTags = 0;
            _tags = tagsData.Tags;

            _tags.ForEach(x => totalTags += x.Count);
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

        private IList<SimplifiedTag> GetSimplifiedTags()
        {
            var simplifiedTagsList = new List<SimplifiedTag>();
            
            foreach (var tag in _tags)
            {
                var simplifiedTag = GetSimplifiedTag(tag);
                simplifiedTagsList.Add(simplifiedTag);
            }

            return simplifiedTagsList;
        }

        private SimplifiedTag GetSimplifiedTag(Tag tag)
        {
            var roundedDivisor = Math.Round(double.Parse(tag.Count.ToString()) / _totalTags, 3);
            var percentage = roundedDivisor * 100;

            return new SimplifiedTag(tag.Name, percentage);
        }
    }
}
