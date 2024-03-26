﻿namespace MediPortSOAPI.TagProcessing
{
    internal struct SimplifiedTag
    {
        public string Name { get; }
        public double Percentage { get; }

        public SimplifiedTag(string name, double percentage)
        {
            Name = name;
            Percentage = percentage;
        }      
    }
}
