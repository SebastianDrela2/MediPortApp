using MediPortApi.SettingsUtils;
using Microsoft.Data.SqlClient;

namespace MediPortApi.Connections
{
    internal class XmlConnectionStringBuilderRetriever
    {
        public SqlConnectionStringBuilder GetSqlConnectionStringBuilder(Settings settings)
        {
            var builder = new SqlConnectionStringBuilder();

            builder.DataSource = settings.ServerName;
            builder.InitialCatalog = settings.DatabaseName;            
            builder.TrustServerCertificate = true;
            builder.ConnectTimeout = 30;           
            builder.ApplicationIntent = ApplicationIntent.ReadWrite;           
            builder.IntegratedSecurity = true;
            builder.Authentication = SqlAuthenticationMethod.NotSpecified;

            if (!string.IsNullOrEmpty(settings.UserName) && !string.IsNullOrEmpty(settings.Password))
            {
                builder.UserID = settings.UserName;
                builder.Password = settings.Password;
                builder.IntegratedSecurity = false;
                builder.Authentication = SqlAuthenticationMethod.SqlPassword;
            }

            return builder;
        }
    }
}
