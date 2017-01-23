using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace signalRClient
{
    class Program
    {
        //public class ShapeModel
        //{
        //    // We declare Left and Top as lowercase with 
        //    // JsonProperty to sync the client and server models
        //    [JsonProperty("left")]
        //    public double Left { get; set; }
        //    [JsonProperty("top")]
        //    public double Top { get; set; }
        //    // We don't want the client to get the "LastUpdatedBy" property
        //    [JsonIgnore]
        //    public string LastUpdatedBy { get; set; }
        //}

        private static void Main(string[] args)
        {
            //Set connection
            var connection = new HubConnection("http://localhost:26096/");
            //Make proxy to hub based on hub name on server
            var myHub = connection.CreateHubProxy("DataTickerHub");
            //Start connection

            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                }
                else {
                    Console.WriteLine("Connected to DataTickerHub.");
                }

            }).Wait();

            //myHub.Invoke<string>("Send", "HELLO World ").ContinueWith(task => {
            //    if (task.IsFaulted)
            //    {
            //        Console.WriteLine("There was an error calling send: {0}",
            //                          task.Exception.GetBaseException());
            //    }
            //    else {
            //        Console.WriteLine(task.Result);
            //    }
            //});

            myHub.On<string>("updateNewData", param => {
                //Console.WriteLine(param.Left+","+param.Top);
                Console.WriteLine(param);
            });

            //myHub.Invoke<string>("UpdateModel", "I'm doing something!!!").Wait();


            Console.Read();
            connection.Stop();
        }
    }
}
