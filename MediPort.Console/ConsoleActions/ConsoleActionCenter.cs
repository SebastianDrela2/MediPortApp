using MediPortApi.HttpProcessing;
using MediPortApi.SqlCommands;
using MediPortApi.TagProcessing;
using Microsoft.Data.SqlClient;

namespace MediPortApi.ConsoleActions
{
    public class ConsoleActionCenter
    {                       
        private readonly StackOverflowService _stackOverflowService;
        private readonly Dictionary<int, Func<Task>> _actions;
        private readonly SqlConnection _connection;

        private DisplaySortedTagsAction _displayAction;      

        public ConsoleActionCenter(SqlConnection connection, TagsData tagsData, StackOverflowService stackOverflowService)
        {
            _connection = connection;
            _stackOverflowService = stackOverflowService;

            SetDisplayAction(tagsData);          
            _actions = new Dictionary<int, Func<Task>>()
            {
                {0, _stackOverflowService.ResetTagsAsync},
                {1, () => { _displayAction!.DisplaySortedTags(SortOption.NameAscending); return Task.CompletedTask; }},
                {2, () => { _displayAction!.DisplaySortedTags(SortOption.NameDescending); return Task.CompletedTask; }},
                {3, () => { _displayAction!.DisplaySortedTags(SortOption.PercentageAscending); return Task.CompletedTask; }},
                {4, () => { _displayAction!.DisplaySortedTags(SortOption.PercentageDescending); return Task.CompletedTask; }}
            };
        }

        public async Task Execute(int actionId)
        {
            _actions.TryGetValue(actionId, out var action);

            if (action is not null)
            {
                await action.Invoke();
            }
            else
            {
                Console.WriteLine($"Invalid action.");
            }
        }

        public void RenderActionList()
        {
            Console.WriteLine();

            Console.WriteLine("Action 0: ResetTags");
            Console.WriteLine("Action 1: DisplaySortedTagsByName");
            Console.WriteLine("Action 2: DisplaySortedTagsByNameDescending");
            Console.WriteLine("Action 3: DisplaySortedTagsByPercentage");
            Console.WriteLine("Action 4: DisplaySortedTagsByPercentageDescending");
            Console.WriteLine("Action 5: Exit");

            Console.WriteLine();
        }
             
        private void SetDisplayAction(TagsData tagsData)
        {
            var simplifiedTagCalculator = new SimplifiedTagCalculator(tagsData.Tags);           
            _displayAction = new DisplaySortedTagsAction(simplifiedTagCalculator);
        }
    }
}
