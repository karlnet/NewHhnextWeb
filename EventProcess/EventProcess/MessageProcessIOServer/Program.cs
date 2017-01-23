using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessIOServer
{
    class Program
    {

        private const string IOServerTokenURI = "http://api.yfiot.com/service/getToken";
        private const string IOServerCmdBaseURI = "http://api.yfiot.com/io/writeIoValues";

        static void Main(string[] args)
        {




            List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>()
           {
                new KeyValuePair<string, string>("token", "d866344682e74051b2310fb2410b6e45" ),
                new KeyValuePair<string, string>("userid", "29"),
                new KeyValuePair<string, string>("projectid", "18"),
                new KeyValuePair<string, string>("values", "1"),
                new KeyValuePair<string, string>("names", "QIA:A1")
           };

           string res= IOServerSetValue(postData).Result;

            Console.WriteLine("Receiving interactive messages from ioserver...{0}", res);
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }


        public static async Task<string> IOServerSetValue(IEnumerable<KeyValuePair<string, string>> postData)
        {
            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync(IOServerCmdBaseURI, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }

                    return null;
                }
               
            }

        }

    }
}
