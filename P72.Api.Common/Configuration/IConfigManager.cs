using Microsoft.Extensions.Configuration;

namespace P72.Api.Common.Configuration
{
    public interface IConfigManager
    {
        string P72Connection { get; }

        string GetConnectionString(string connectionName);

        IConfigurationSection GetConfigurationSection(string key);
    }
}
