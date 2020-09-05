using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisServices
    {
        private readonly string _redisHost;
        private readonly string _redisPort;

        //connectionmultiplixer classı sayesinde redis serverla haberleşicez.
        private ConnectionMultiplexer _redis;
        
        public IDatabase db { get; set; }
        public RedisServices(IConfiguration configuration)
        {

            //appsetting.json'daki redisin altındaki hosta eriştik.
            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }
        //redis serverla haberleşicek metodu tanımlıyoruz.
        public void Connect()
        {
            var configString = $"{ _redisHost}:{ _redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);
        }
        //redisdesktop managerdaki bir db yi seçiyoruz.
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
