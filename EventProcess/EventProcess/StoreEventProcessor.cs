using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IotHubEventProcess
{
    class StoreEventProcessor : IEventProcessor
    {
        private const int MAX_BLOCK_SIZE = 4 * 1024 * 1024;
        public static string StorageConnectionString;
        public static string ServiceBusConnectionString;

        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        //private QueueClient queueClient;
        private TopicClient topicClient;

        private long currentBlockInitOffset;
        private MemoryStream toAppend = new MemoryStream(MAX_BLOCK_SIZE);

        private Stopwatch stopwatch;
        private TimeSpan MAX_CHECKPOINT_TIME = TimeSpan.FromHours(1);

        public StoreEventProcessor()
        {
            var storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference("d2ctutorial");
            blobContainer.CreateIfNotExists();
            //queueClient = QueueClient.CreateFromConnectionString(ServiceBusConnectionString);
            topicClient =    TopicClient.CreateFromConnectionString(ServiceBusConnectionString, "hhnext-topic");
        }

        async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.", context.Lease.PartitionId, reason);
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
            //return Task.FromResult<object>(null);
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            Console.WriteLine("StoreEventProcessor initialized.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset);

            if (!long.TryParse(context.Lease.Offset, out currentBlockInitOffset))
            {
                currentBlockInitOffset = 0;
            }
            stopwatch = new Stopwatch();
            stopwatch.Start();

            return Task.FromResult<object>(null);
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                byte[] data = eventData.GetBytes();
                var messageId = (string)eventData.SystemProperties["message-id"];

                if (eventData.Properties.ContainsKey("messageType") && (string)eventData.Properties["messageType"] == "interactive")
                {

                    var bodyStream = new MemoryStream(data);
                    bodyStream.Position = 0;
                    var bodyAsString = new StreamReader(bodyStream, Encoding.ASCII).ReadToEnd();

                    JObject bodyAsJson = JObject.Parse(bodyAsString);
                    bodyAsJson.AddFirst(new JProperty("ProjectName", "test"));

                    dynamic newBodyAsJson = bodyAsJson;
                    newBodyAsJson.ProjectName = "test";


                    WriteHighlightedMessage(string.Format("messageId : {0}", messageId));
                    WriteHighlightedMessage(string.Format("message: {0}", newBodyAsJson.ToString()));

                    string messageBodyAsString = JsonConvert.SerializeObject(newBodyAsJson);
                    var messageBodyStream = new MemoryStream(Encoding.UTF8.GetBytes(messageBodyAsString));

                    BrokeredMessage topicMessage = new BrokeredMessage(messageBodyStream);
                    topicMessage.MessageId = messageId;
                    topicMessage.Properties["messageType"] = "interactive";

                    await topicClient.SendAsync(topicMessage);

                    continue;
                }

                //if (toAppend.Length + data.Length > MAX_BLOCK_SIZE || stopwatch.Elapsed > MAX_CHECKPOINT_TIME)
                //{
                //    await AppendAndCheckpoint(context);
                //}
                //await toAppend.WriteAsync(data, 0, data.Length);

                

                Console.WriteLine(string.Format("Message received.  Partition: '{0}', Data: '{1}'",
                  context.Lease.PartitionId, Encoding.UTF8.GetString(data)));
            }

            //if (this.stopwatch.Elapsed > TimeSpan.FromMinutes(5))
            //{
                await context.CheckpointAsync();
                this.stopwatch.Restart();
            //}

        }

        private async Task AppendAndCheckpoint(PartitionContext context)
        {
            var blockIdString = String.Format("startSeq:{0}", currentBlockInitOffset.ToString("0000000000000000000000000"));
            var blockId = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(blockIdString));
            toAppend.Seek(0, SeekOrigin.Begin);
            byte[] md5 = MD5.Create().ComputeHash(toAppend);
            toAppend.Seek(0, SeekOrigin.Begin);

            var blobName = String.Format("iothubd2c_{0}", context.Lease.PartitionId);
            var currentBlob = blobContainer.GetBlockBlobReference(blobName);

            if (await currentBlob.ExistsAsync())
            {
                await currentBlob.PutBlockAsync(blockId, toAppend, Convert.ToBase64String(md5));
                var blockList = await currentBlob.DownloadBlockListAsync();
                var newBlockList = new List<string>(blockList.Select(b => b.Name));

                if (newBlockList.Count() > 0 && newBlockList.Last() != blockId)
                {
                    newBlockList.Add(blockId);
                    WriteHighlightedMessage(String.Format("Appending block id: {0} to blob: {1}", blockIdString, currentBlob.Name));
                }
                else
                {
                    WriteHighlightedMessage(String.Format("Overwriting block id: {0}", blockIdString));
                }
                await currentBlob.PutBlockListAsync(newBlockList);
            }
            else
            {
                await currentBlob.PutBlockAsync(blockId, toAppend, Convert.ToBase64String(md5));
                var newBlockList = new List<string>();
                newBlockList.Add(blockId);
                await currentBlob.PutBlockListAsync(newBlockList);

                WriteHighlightedMessage(String.Format("Created new blob", currentBlob.Name));
            }

            toAppend.Dispose();
            toAppend = new MemoryStream(MAX_BLOCK_SIZE);

            // checkpoint.
            await context.CheckpointAsync();
            WriteHighlightedMessage(String.Format("Checkpointed partition: {0}", context.Lease.PartitionId));

            currentBlockInitOffset = long.Parse(context.Lease.Offset);
            stopwatch.Restart();
        }

        private void WriteHighlightedMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
