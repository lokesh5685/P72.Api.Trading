using Microsoft.Extensions.Configuration;

namespace P72.Api.Common.Configuration
{
    public class ConfigManager : IConfigManager
    {
        private readonly IConfiguration? _configuration;
        public ConfigManager(IConfiguration? configuration)
        {
            this._configuration = configuration;
        }
        public string P72Connection
        {
            get
            {
                return this._configuration?["ConnectionStrings:DBEntities"]!;
            }
        }

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return this._configuration!.GetSection(key);
        }

        public string GetConnectionString(string connectionName)
        {
            return this._configuration?.GetConnectionString(connectionName)!;
        }


    }
}
