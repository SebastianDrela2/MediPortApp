namespace MediPortApi.TagProcessing
{
    public struct SimplifiedTag
    {
        public string Name { get; }
        public int Count { get; }
        public double Percentage { get; }

        public SimplifiedTag(string name, int count, double percentage)
        {
            Name = name;
            Count = count;
            Percentage = percentage;
        }      
    }
}
