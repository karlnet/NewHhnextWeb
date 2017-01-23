using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.ServiceBus.Messaging;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessagesProcessDocumentDB
{
    class Program
    {


        private const string DBNAME = "hhnext-documentDB";
        private const string DBCOLLECTION = "test";
        private const string PrimaryKey = "aP7q0TTQ4iLmUArRojW3xx88lErJrVshte8qzNZIraZY9TA8dasSUTZ8yhTzR18qIvnqGYvvId4k8pQvsVGufQ==";
        private const string EndpointUri = "https://hhnext.documents.azure.com:443/";

        private const string connectionString =
                  "Endpoint=sb://hhnext.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9HTDDNPcUjr4fNYRf1//VWRVh/k0a4b/tifTCEvwZjw=";

        
        private DocumentClient dbClient;

        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            // Check to verify a database with the id=FamilyDB does not exist
            try
            {
                await this.dbClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
                this.WriteToConsoleAndPromptToContinue("Found {0}", databaseName);
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.dbClient.CreateDatabaseAsync(new Database { Id = databaseName });
                    this.WriteToConsoleAndPromptToContinue("Created {0}", databaseName);
                }
                else
                {
                    throw;
                }
            }
        }

        private  async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            try
            {
                await this.dbClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
                this.WriteToConsoleAndPromptToContinue("Found {0}", collectionName);
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    // Configure collections for maximum query flexibility including string range queries.
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    // Here we create a collection with 400 RU/s.
                    await this.dbClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        collectionInfo,
                        new RequestOptions { OfferThroughput = 400 });

                    this.WriteToConsoleAndPromptToContinue("Created {0}", collectionName);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task GetStartedDemo()
        {
            this.dbClient = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            await this.CreateDatabaseIfNotExists(DBNAME);
            await this.CreateDocumentCollectionIfNotExists(DBNAME, DBCOLLECTION);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Process D2C Interactive Messages app\n");
            Program p = new Program();

            try
            {
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                //Console.WriteLine("End of document.");
                //Console.ReadKey();
            }

            //QueueClient Client = QueueClient.CreateFromConnectionString(connectionString);
            SubscriptionClient subscriptionClient = SubscriptionClient.CreateFromConnectionString(connectionString, "hhnext-topic", "hhnext-subscribe-documentDB");

            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            subscriptionClient.OnMessage(  (message) =>
            {
                try
                {
                    var bodyStream = message.GetBody<Stream>();
                    bodyStream.Position = 0;
                    var bodyAsString = new StreamReader(bodyStream, Encoding.ASCII).ReadToEnd();
                    JObject bodyAsJson = JObject.Parse(bodyAsString);

                    Console.WriteLine("MessageId: {0} ", message.MessageId);
                    Console.WriteLine("message: {0} ", bodyAsString);

                    string DBCollention = (string)bodyAsJson["ProjectName"];
                    bodyAsJson.Property("ProjectName").Remove();

                    p.dbClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DBNAME, DBCollention), bodyAsJson);

                    message.Complete();
                }
                catch (Exception e)
                {
                    Console.WriteLine("error :  {0} ", e.ToString());
                    message.Abandon();
                }
            }, options);
            Console.WriteLine("Receiving interactive messages from SB queue...");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, args);
            //Console.WriteLine("Press any key to continue ...");
            //Console.ReadKey();
            Console.ResetColor();
        }
    }
}
