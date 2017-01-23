using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;

namespace hhnextWeb
{
    public class RedisDataTicker
    {

        public  ConcurrentDictionary<string, HashSet<string>> userConnections = new ConcurrentDictionary<string, HashSet<string>>();

        // signalR hub ' Singleton instance
        private readonly static Lazy<RedisDataTicker> _instance = new Lazy<RedisDataTicker>(() => new RedisDataTicker(GlobalHost.ConnectionManager.GetHubContext<DataTickerHub>().Clients));

        private IHubConnectionContext<dynamic> signalRHubClients { get; set; }

        // redis client
        private ISubscriber subscriber = Helper.Connection.GetSubscriber();
        private IDatabase   redisCache = Helper.Connection.GetDatabase();
        private const string SingleRChannel = "signalRChannel";

        // service bus queue client
        private const string queueConnectionString
            = "Endpoint=sb://hhnext.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9HTDDNPcUjr4fNYRf1//VWRVh/k0a4b/tifTCEvwZjw=";
        private const string queueName = "hhnext-queue";
        private QueueClient queueClient;

        public static RedisDataTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private RedisDataTicker(IHubConnectionContext<dynamic> clients)
        {
            signalRHubClients = clients;

            subscriber.Subscribe(SingleRChannel, (channel, message) =>
            {
                BroadcastNewData((string)message);
            });

            queueClient = QueueClient.CreateFromConnectionString(queueConnectionString, queueName);

        }
        private string GetAllDataFromRedis(string name)
        {
            var projectId = redisCache.HashGet("User:Project", "zhangxb");
            var projectDevices = redisCache.ListRange(projectId + ":Devices");
            IEnumerable<string> data = projectDevices
                            .Select(item =>
                          {
                              string deviceId = item.ToString();
                              var deviceData = redisCache.HashGetAll(deviceId).ToStringDictionary();
                              deviceData.Add("DeviceId", deviceId);
                              var newDeviceData = JsonConvert.SerializeObject(deviceData);
                              return newDeviceData;
                          });

            return JsonConvert.SerializeObject(data);
        }

        public void SendCommandToQueue(string name,string data)
        {
            JObject bodyAsJson = JObject.Parse(data);
            bodyAsJson.AddFirst(new JProperty("UserName", "orinoco"));   // test user orinoco
            var message = new BrokeredMessage(JsonConvert.SerializeObject(bodyAsJson));
            queueClient.Send(message);

        }

        


        public void GetAllData(string name)   // GetAllData  is  method for brower call throught  hub
        {
            var connectionIds = userConnections[name];
            string res = GetAllDataFromRedis(name);

            foreach (var connectionId in connectionIds)
            {
                signalRHubClients.Client(connectionId).updateAllData(res);
            }
          
        }

        private void BroadcastNewData(string data)
        {
            signalRHubClients.All.updateNewData(data);    //updateNewData  is browser js method
        }
    }
}