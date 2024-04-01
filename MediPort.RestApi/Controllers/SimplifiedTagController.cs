using MediPort.RestApi.Data;
using MediPortApi.HttpProcessing;
using MediPortApi.TagProcessing;
using Microsoft.AspNetCore.Mvc;

namespace MediPort.RestApi.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class SimplifiedTagController : ControllerBase
    {
        private readonly ITagsStore _tagsStore;

        public SimplifiedTagController(ITagsStore tagsStore)
        {
            _tagsStore = tagsStore;
        }

        [HttpGet("data/{apiKey}")]
        public async Task<ActionResult<IEnumerable<SimplifiedTag>>> RefreshAllTags(string apiKey)
        {
            await _tagsStore.RefreshAllTags(apiKey);

            return Ok("Tags have been refreshed successfully.");
        }

        [HttpGet("results")]
        public async Task<ActionResult<IEnumerable<SimplifiedTag>>> GetSimplifiedTagsSorted(int page = 1, string sort = "nameascending")
        {
            var sortOption = GetSortOption(sort);
            var sortedTagItems = await _tagsStore.GetTagsSorted(page, sortOption);

            return Ok(sortedTagItems);
        }
        
        [HttpGet("{id}", Name = "GetSimplifiedTagById")]
        public async Task<ActionResult<IEnumerable<SimplifiedTag>>> GetSimplifiedTagById(int id)
        {
            var tagItem = await _tagsStore.GetTag(id);

            if (tagItem.Percentage is not 0 && !string.IsNullOrEmpty(tagItem.Name))
            {
                return Ok(tagItem);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<SimplifiedTag>> CreateTag(Tag tag)
        {
            await _tagsStore.CreateTag(tag);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTag(int id, Tag tag)
        {
            await _tagsStore.UpdateTag(id, tag);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            await _tagsStore.DeleteTag(id);

            return NoContent();
        }

        private SortOption GetSortOption(string value)
        {
            value = value.ToLower();

            return value switch
            {
                "nameascending" => SortOption.NameAscending,
                "namedescending" => SortOption.NameDescending,
                "percentageascending" => SortOption.PercentageAscending,
                "percentagedescending" => SortOption.PercentageDescending,
                "countascending" => SortOption.CountAscending,
                "countdescending" => SortOption.CountDescending,
                _ => throw new ArgumentException($"Invalid sort option: {value}")
            };
        }
    }
}
