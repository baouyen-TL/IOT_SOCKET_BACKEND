using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public class ConfigManager
    {
        private readonly IConfiguration _configuration;
        public ConfigManager()
        {
            this._configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();
        }

        public string DefaultConnection
        {
            get
            {
                return this._configuration.GetConnectionString("DefaultConnection");
            }
        }

        public string BrokerAddress
        {
            get
            {
                return this._configuration["MQTTX:BrokerAddress"];
            }
        }

        public int Port
        {
            get
            {
                return int.Parse(this._configuration["MQTTX:Port"]);
            }
        }

        public string UserName
        {
            get
            {
                return this._configuration["MQTTX:UserName"];
            }
        }

        public string Password
        {
            get
            {
                return this._configuration["MQTTX:Password"];
            }
        }

    }
}
