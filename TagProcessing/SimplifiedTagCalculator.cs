using MediPortSOAPI.HttpProcessing;

namespace MediPortSOAPI.TagProcessing
{
    internal class SimplifiedTagCalculator
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

        private SimplifiedTag GetSimplifiedTag(Tag tag)
        {
            var roundedDivisor = Math.Round(double.Parse(tag.Count.ToString()) / _totalTags, 3);
            var percentage = roundedDivisor * 100;

            return new SimplifiedTag(tag.Name, percentage);
        }
    }
}
