using MediPortApi.SettingsUtils;
using Microsoft.Data.SqlClient;

namespace MediPortApi.Connections
{
    public static class SqlConnectionFactory
    {
        public static SqlConnection GetSqlConnection(Settings settings)
        {
            var builderRetriever = new XmlConnectionStringBuilderRetriever();
            var connectionStringBuilder = builderRetriever.GetSqlConnectionStringBuilder(settings);

            return new SqlConnection(connectionStringBuilder.ConnectionString);
        }
    }
}
