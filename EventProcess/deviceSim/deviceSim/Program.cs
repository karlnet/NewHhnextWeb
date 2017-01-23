using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace deviceSim
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=hhnext.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=jjHgtQz3AtfnTn6p/I5zH9POHLg9f55WnYlPD4y0Sqw=";
        static string iotHubUri = "hhnext.azure-devices.net";
        static string deviceId = null;
        static int maxTemperature = 0;
        static int minTemperature = 1;
        static string deviceKey = null;
        static DeviceClient deviceClient = null;
        static Random random = new Random();

        static string GenerateMessage(int seq, string message)
        {
            var msg = TelemetryData.Random(string.Format("{0}{1}", DateTime.UtcNow.ToString("yyyymmdd"), seq.ToString("000")), message);
            return JsonConvert.SerializeObject(msg);
        }
        static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Log(string msg)
        {
            Console.ResetColor();
            Console.WriteLine(msg);
        }
        static void Wait(string msg = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Log("Press [ENTER] to continue...");
            }
            else
            {
                Log(msg);
            }
            Console.ReadLine();
        }
        private async static Task AddDeviceAsync()
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            deviceKey = device.Authentication.SymmetricKey.PrimaryKey;
            Log($"device id {deviceId} : {deviceKey}");
        }
        private async static Task RemoveDeviceAsync()
        {
            var device = await registryManager.GetDeviceAsync(deviceId);
            await registryManager.RemoveDeviceAsync(device);
        }
        private static async void SendDeviceToCloudMessagesAsync()
        {
            int i = 0;
            while (true)
            {
                i++;
                string telemetry = GenerateMessage(i, $"message:{i}");                
                var interactiveMessage = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(telemetry));

                interactiveMessage.Properties["messageType"] = "interactive";
                interactiveMessage.MessageId = Guid.NewGuid().ToString();

                await deviceClient.SendEventAsync(interactiveMessage);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, telemetry);

                Task.Delay(5000).Wait();
            }
        }
        private async static void ReceiveCommandAsync()
        {
            while (true)
            {
                var cmd = await deviceClient.ReceiveAsync();
                if (cmd != null)
                {
                    Success(Encoding.UTF8.GetString(cmd.GetBytes()));
                }

                Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Error("Usage: sumulator {deviceid} {min} {max}");
                Wait();
                return;
            }
            deviceId = args[0];
            minTemperature = int.Parse(args[1]);
            maxTemperature = int.Parse(args[2]);

            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            //deviceKey = "OTUJwGXWV6mweq/CUSlqaEnackTI6SYXBYM3U75HbKg=";

#if true
            //AMQP (default)
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey));
#else
            //HTTPS
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                                                Microsoft.Azure.Devices.Client.TransportType.Http1);
#endif
            SendDeviceToCloudMessagesAsync();
            ReceiveCommandAsync();
            Wait("Press [ENTER] to exit...");

            RemoveDeviceAsync().Wait();

        }
    }
}
