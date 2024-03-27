using MediPortApi.HttpProcessing;

namespace Mediport.Api.Tests
{
    internal class MockTagData
    {
        private readonly int _limiter;
        public MockTagData(int limiter)
        {
            _limiter = limiter;
        }

        public TagsData GetMockTagsData()
        {
            var mockTagData = new TagsData();           

            for (var i = 0; i <= _limiter; i++)
            {
                mockTagData.Tags.Add(new Tag
                {
                    Name = $"tag{i}",
                    Count = i,
                    IsModeratorOnly = true,
                    IsRequired = false
                });
            }

            return mockTagData;
        }
    }
}
