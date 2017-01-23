using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MessageProcessRedis
{
    class Helper
    {
        
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisCacheName"] + ",abortConnect=false,ssl=true,password=" + ConfigurationManager.AppSettings["RedisCachePassword"]);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

    }
}
