using System;
using System.IO;
using System.Linq;
using System.Threading;
using Args;
using CsvHelper;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace IndexUpdater
{
    internal class Program
    {
        private const string IndexName = "pubs";
        private const string SearchServiceName = "londonpubs";

        private static void Main(string[] args)
        {
            var command = Configuration.Configure<CommandObject>().CreateAndBind(args);
            var apiKey = command.ApiKey;

            try
            {
                RunUpdate(apiKey, command);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void RunUpdate(string apiKey, CommandObject command)
        {
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                CredentialManager.SetApiKey(apiKey);
            }
            else
            {
                apiKey = CredentialManager.GetApiKey();
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                while (string.IsNullOrWhiteSpace(apiKey))
                {
                    Console.Out.WriteLine("API Key - Stored in Windows Credential Manager:");
                    apiKey = Console.ReadLine();
                }

                CredentialManager.SetApiKey(apiKey);
            }

            var pubs = GetPubsFromFile(command.File);

            CreateIndex(apiKey);

            UpdateIndex(apiKey, pubs);
        }

        private static void CreateIndex(string apiKey)
        {
            var definition = new Index()
            {
                Name = IndexName,
                Fields = FieldBuilder.BuildForType<IndexablePub>()
            };

            using (var searchServiceClient = CreateSearchServiceClient(apiKey))
            {
                Console.Out.WriteLine($"Deleting Index");
                searchServiceClient.Indexes.Delete(IndexName);
                Thread.Sleep(5000);
                Console.Out.WriteLine($"Creating Index");
                searchServiceClient.Indexes.Create(definition);
                Thread.Sleep(5000);
            }
        }


        private static Pub[] GetPubsFromFile(string pubFileLocation)
        {
            using (var reader = new StreamReader(pubFileLocation))
            using (var csv = new CsvReader(reader))
            {
                return csv.GetRecords<Pub>().ToArray();
            }
        }

        private static void UpdateIndex(string apiKey, Pub[] pubs)
        {
            using (var searchServiceClient = CreateSearchServiceClient(apiKey))
            {
                using (var indexClient = searchServiceClient.Indexes.GetClient(IndexName))
                {
                    var enumerable = pubs.Select(BuildIndexablePub).Where(action => action != null);
                    var batch = IndexBatch.New(enumerable);

                    try
                    {
                        indexClient.Documents.Index(batch);
                    }
                    catch (IndexBatchException e)
                    {
                        // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                        // the batch. Depending on your application, you can take compensating actions like delaying and
                        // retrying. For this simple demo, we just log the failed document keys and continue.
                        Console.WriteLine(
                            "Failed to index some of the documents: {0}",
                            string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                    }
                }
            }
        }

        private static IndexAction<IndexablePub> BuildIndexablePub(Pub pub)
        {

            try
            {
                return IndexAction.Upload(new IndexablePub(pub));
            }
            catch (Exception exception)
            {
                Console.Out.WriteLine($"Failed to convert: {pub.Name} - Error: {exception}");
                return null;
            }
        }

        private static SearchServiceClient CreateSearchServiceClient(string apiKey)
        {
            var serviceClient = new SearchServiceClient(SearchServiceName, new SearchCredentials(apiKey));
            return serviceClient;
        }
    }
}