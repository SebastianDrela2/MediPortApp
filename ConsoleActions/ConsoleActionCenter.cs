using MediPortSOAPI.HttpProcessing;
using MediPortSOAPI.SqlCommands;
using MediPortSOAPI.TagProcessing;
using Microsoft.Data.SqlClient;

namespace MediPortSOAPI.ConsoleActions
{
    internal class ConsoleActionCenter
    {                       
        private readonly StackOverflowService _stackOverflowService;
        private readonly Dictionary<int, Func<Task>> _actions;
        private readonly SqlConnection _connection;

        private DisplaySortedTagsAction _displayAction;
        private SimplifiedTagCalculator _simplifiedTagCalculator;

        public ConsoleActionCenter(SqlConnection connection, TagsData tagsData, StackOverflowService stackOverflowService)
        {
            _connection = connection;
            _simplifiedTagCalculator = new SimplifiedTagCalculator(tagsData);
            _stackOverflowService = stackOverflowService;

            var simplifiedTags = _simplifiedTagCalculator.GetSimplifiedTags();                                        
            _displayAction = new DisplaySortedTagsAction(simplifiedTags);

            var repopulateTableCommand = new PopulateTagsTableCommand(connection);

            _actions = new Dictionary<int, Func<Task>>()
            {
                {0, ResetTags},
                {1, () => { _displayAction.DisplaySortedAscendingByName(); return Task.CompletedTask; }},
                {2, () => { _displayAction.DisplaySortedDescendingByName(); return Task.CompletedTask; }},
                {3, () => { _displayAction.DisplaySortedAscendingPercentage(); return Task.CompletedTask; }},
                {4, () => { _displayAction.DisplaySortedDescendingByPercentage(); return Task.CompletedTask; }}
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

            Console.WriteLine("Action 0: RefetchTags");
            Console.WriteLine("Action 1: DisplaySortedTagsByName");
            Console.WriteLine("Action 2: DisplaySortedTagsByNameDescending");
            Console.WriteLine("Action 3: DisplaySortedTagsByPercentage");
            Console.WriteLine("Action 4: DisplaySortedTagsByPercentageDescending");
            Console.WriteLine("Action 5: Exit");

            Console.WriteLine();
        }
        
        private async Task ResetTags()
        {
            var tagsData = await _stackOverflowService.GetTagsDataAsync();
            _simplifiedTagCalculator = new SimplifiedTagCalculator(tagsData);

            var simplifiedTags = _simplifiedTagCalculator.GetSimplifiedTags();
            _displayAction = new DisplaySortedTagsAction(simplifiedTags);

            var deleteTableCommand = new DeleteTagsTableCommand(_connection);
            deleteTableCommand.Execute();

            var populateTableCommand = new PopulateTagsTableCommand(_connection);
            populateTableCommand.Execute(tagsData);
        }

    }
}
