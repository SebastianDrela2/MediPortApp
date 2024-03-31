using MediPortApi.TagProcessing;

namespace Mediport.Api.Tests
{
    [TestFixture]
    internal class TagProcessingTestFixture
    {
        [TestCase(SortOption.NameAscending, "tag0")]
        [TestCase(SortOption.NameDescending, "tag9")]
        public void SimplifiedTagsAreSortedCorrectlyByName(SortOption sortOption, string expected)
        {
            var limiter = 10;
            var mockTagData = new MockTagData(limiter).GetMockTagsData();

            var calculator = new SimplifiedTagCalculator(mockTagData.Tags);
            var simplifiedTags = calculator.GetSortedSimplifiedTags(sortOption);

            Assert.That(simplifiedTags.First().Name, Is.EqualTo(expected));
        }

        [TestCase(SortOption.PercentageAscending, 0)]
        [TestCase(SortOption.PercentageDescending, 18.2)]
        public void SimplifiedTagsAreSortedCorrectlyByPercentage(SortOption sortOption, double expected)
        {
            var limiter = 10;
            var mockTagData = new MockTagData(limiter).GetMockTagsData();

            var calculator = new SimplifiedTagCalculator(mockTagData.Tags);
            var simplifiedTags = calculator.GetSortedSimplifiedTags(sortOption);

            Assert.That(simplifiedTags.First().Percentage, Is.EqualTo(expected));
        }
    }
}
