using MediPortSOAPI.HttpProcessing;
using MediPortSOAPI.SqlCommands;
using MediPortSOAPI.TagProcessing;
using Microsoft.Data.SqlClient;

namespace MediPortSOAPI.ConsoleActions
{
    internal class ConsoleActionCenter
    {
        private readonly SqlConnection _connection;
        private readonly StackOverflowService _stackOverflowService;
        private readonly Dictionary<int, Func<Task>> _actions;       

        private DisplaySortedTagsAction _displayAction;      

        public ConsoleActionCenter(SqlConnection connection, TagsData tagsData, StackOverflowService stackOverflowService)
        {
            _connection = connection;
            _stackOverflowService = stackOverflowService;

            SetDisplayAction(tagsData);          
            _actions = new Dictionary<int, Func<Task>>()
            {
                {0, ResetTagsAsync},
                {1, () => { _displayAction!.DisplaySortedAscendingByName(); return Task.CompletedTask; }},
                {2, () => { _displayAction!.DisplaySortedDescendingByName(); return Task.CompletedTask; }},
                {3, () => { _displayAction!.DisplaySortedAscendingPercentage(); return Task.CompletedTask; }},
                {4, () => { _displayAction!.DisplaySortedDescendingByPercentage(); return Task.CompletedTask; }}
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
        
        private async Task ResetTagsAsync()
        {
            var tagsData = await _stackOverflowService.GetTagsDataAsync();
            SetDisplayAction(tagsData);

            var deleteTableCommand = new DeleteTagsTableCommand(_connection);
            deleteTableCommand.Execute();

            var populateTableCommand = new PopulateTagsTableCommand(_connection);
            populateTableCommand.Execute(tagsData);
        }

        private void SetDisplayAction(TagsData tagsData)
        {
            var simplifiedTagCalculator = new SimplifiedTagCalculator(tagsData);
            var simplifiedTags = simplifiedTagCalculator.GetSimplifiedTags();

            _displayAction = new DisplaySortedTagsAction(simplifiedTags);
        }
    }
}
