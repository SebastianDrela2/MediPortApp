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

        [HttpGet("config/{apiKey}")]
        public async Task<ActionResult<IEnumerable<SimplifiedTag>>> RefreshAllTags(string apiKey)
        {
            await _tagsStore.RefreshAllTags(apiKey);
            var refreshedSimplifedTags = await GetAllSimplifiedTags();

            if (refreshedSimplifedTags.Result is OkObjectResult okObjectResult)
            {
                return Ok(okObjectResult.Value);
            }

            return NoContent();
        }

        [HttpGet("results/{sort}")]
        public async Task<ActionResult<IEnumerable<SimplifiedTag>>> GetAllSimplifiedTagsSorted(string sort)
        {
            var sortOption = GetSortOption(sort);
            var sortedTagItems = await _tagsStore.GetAllTagsSorted(sortOption);

            return Ok(sortedTagItems);
        }


        [HttpGet]
        public async Task<ActionResult <IEnumerable<SimplifiedTag>>> GetAllSimplifiedTags()
        {
            var tagItems = await _tagsStore.GetAllTags();

            return Ok(tagItems);
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
                _ => throw new ArgumentException($"Invalid sort option: {value}")
            };
        }
    }
}
