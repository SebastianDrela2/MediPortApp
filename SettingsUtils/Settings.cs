namespace MediPortSOAPI.SettingsUtils
{
    public class Settings
    {
        public string ServerName { get; }
        public string DatabaseName { get; }
        public string UserName { get; }
        public string Password { get; }
        public string StackOverFlowApiKey { get; }

        public Settings(string serverName, string databaseName, string userName, string password, string stackOverFlowApiKey)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
            StackOverFlowApiKey = stackOverFlowApiKey;
        }
    }
}
