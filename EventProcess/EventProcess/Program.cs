using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
namespace IotHubEventProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            string iotHubConnectionString = "HostName=hhnext.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=jjHgtQz3AtfnTn6p/I5zH9POHLg9f55WnYlPD4y0Sqw=";
            string iotHubD2cEndpoint = "messages/events";
            StoreEventProcessor.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=hhnext;AccountKey=taWiGCrW5t0zmb2gjGcR3Dzy3t47B8BV2V9ibX0thturZGCYDHxlVOHx/ZRQ+8mYSAffaWxoxTjlvXt5kdThBA==;";
            StoreEventProcessor.ServiceBusConnectionString =
                "Endpoint=sb://hhnext.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9HTDDNPcUjr4fNYRf1//VWRVh/k0a4b/tifTCEvwZjw=";

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, iotHubD2cEndpoint, EventHubConsumerGroup.DefaultGroupName, iotHubConnectionString, StoreEventProcessor.StorageConnectionString, "messages-events");
            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<StoreEventProcessor>().Wait();

            Console.WriteLine("Receiving ,  Press enter key to stop worker.");
            Console.ReadLine();

            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
