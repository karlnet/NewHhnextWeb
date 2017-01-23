using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackExchange.Redis;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;

namespace QueueCommandProcess
{
    class Program
    {
        // IOT hub
        static string connectionString 
            = "HostName=hhnext.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=jjHgtQz3AtfnTn6p/I5zH9POHLg9f55WnYlPD4y0Sqw=";

        // documentDB 
        private const string DBNAME = "hhnext-documentDB";
        private const string DBCOLLECTION = "test";
        private const string PrimaryKey = "aP7q0TTQ4iLmUArRojW3xx88lErJrVshte8qzNZIraZY9TA8dasSUTZ8yhTzR18qIvnqGYvvId4k8pQvsVGufQ==";
        private const string EndpointUri = "https://hhnext.documents.azure.com:443/";

        // service bus queue 
        private const string queueConnectionString
            = "Endpoint=sb://hhnext.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9HTDDNPcUjr4fNYRf1//VWRVh/k0a4b/tifTCEvwZjw=";
        private const string queueName = "hhnext-queue";

        static QueueClient queueClient;
        static DocumentClient dbClient;
        static ServiceClient serviceClient;

        private async static Task SendCloudToDeviceMessageAsync(JObject messagebodyAsJson)
        {
            //JObject command = (JObject)messagebodyAsJson["Command"];
            string deviceId = (string)messagebodyAsJson["DeviceId"];

            string messageBodyAsString = JsonConvert.SerializeObject(messagebodyAsJson);
            Console.WriteLine("message to device:{0}\n", messagebodyAsJson.ToString());

            var commandMessage = new Message(Encoding.ASCII.GetBytes(messageBodyAsString));
            commandMessage.Ack = DeliveryAcknowledgement.Full;

            await serviceClient.SendAsync(deviceId, commandMessage);
        }
        private async static void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Receiving command from queue...");
            Console.WriteLine("Press any key to exit.");

            queueClient = QueueClient.CreateFromConnectionString(queueConnectionString, queueName);
            dbClient = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            ReceiveFeedbackAsync();

            queueClient.OnMessage(message =>
            {
                string messageBody = message.GetBody<String>();
                JObject messagebodyAsJson = JObject.Parse(messageBody);

                var timestamp = string.Format("{0:G}", DateTime.Now);
                messagebodyAsJson.AddFirst(new JProperty("Timestamp", timestamp));

                //send to iothub , then deliver to device
                Console.WriteLine("Send Cloud-to-Device message\n");
                SendCloudToDeviceMessageAsync(messagebodyAsJson).Wait();

               

                //add to documentDB
                dbClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DBNAME, DBCOLLECTION), messagebodyAsJson);

                Console.WriteLine(String.Format("Message body: {0}", messagebodyAsJson.ToString()));
                Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
            });

            Console.ReadLine();

        }

    }
}
