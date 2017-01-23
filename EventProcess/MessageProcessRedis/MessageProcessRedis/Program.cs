using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace MessageProcessRedis
{
    class Program
    {

        private const string SingleRChannel= "signalRChannel";

        static void Main(string[] args)
        {
            IDatabase redisCache = Helper.Connection.GetDatabase();
            ISubscriber redisSub = Helper.Connection.GetSubscriber();

            var connString = ConfigurationManager.AppSettings["ServiceBusconnectionString"];

            SubscriptionClient subscriptionClient = SubscriptionClient.CreateFromConnectionString(connString, "hhnext-topic", "hhnext-subscribe-redis");

            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            subscriptionClient.OnMessage((message) =>
            {
                try
                {
                    var bodyStream = message.GetBody<Stream>();
                    bodyStream.Position = 0;
                    var bodyAsString = new StreamReader(bodyStream, Encoding.ASCII).ReadToEnd();

                    JObject bodyAsJson = JObject.Parse(bodyAsString);

                    Console.WriteLine("MessageId: {0} ", message.MessageId);
                    Console.WriteLine("message: {0} ", bodyAsString);

                    string projectName = (string)bodyAsJson["ProjectName"];
                    string deviceId = (string)bodyAsJson["DeviceId"];
                    JObject data = (JObject)bodyAsJson["Data"];

                    List<HashEntry> hes = new List<HashEntry>();
                    //StringBuilder sb = new StringBuilder();

                    foreach (JProperty property in data.Properties())
                    {
                        //sb.Append(","+property.Name + ":" + property.Value.ToString());
                        //hes.Add(new  HashEntry(deviceId+":"+property.Name, property.Value.ToString()));
                        hes.Add(new HashEntry(property.Name, property.Value.ToString()));

                    }

                    redisCache.HashSet(deviceId,hes.ToArray());

                    dynamic newData = new JObject();
                    newData.DeviceId = deviceId;
                    newData.Data = data;

                    Console.WriteLine("signal message : {0} ", newData.ToString());

                    string messageBodyAsString = JsonConvert.SerializeObject(newData);
                    redisSub.Publish(SingleRChannel, messageBodyAsString);

                    message.Complete();
                }
                catch (Exception e)
                {
                    message.Abandon();
                }
            }, options);
            Console.WriteLine("Receiving interactive messages from SB queue...");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }


    }
}
