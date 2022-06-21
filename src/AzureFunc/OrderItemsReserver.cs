using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFunc
{
    public static class OrderItemsReserver
    {
        [FunctionName("OrderItemsReserver")]
        public static async Task Run([ServiceBusTrigger("orderQueue", Connection = "ServiceBusConnection")] string myQueueItem, ILogger log)
        {
            //just create servisbus + queue(orderQueue)
            // create storage acc + blob container
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            try
            {
                string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                string containerName = Environment.GetEnvironmentVariable("ContainerName");
                var blobClient = new BlobContainerClient(connection, containerName);
                var newGuid = System.Guid.NewGuid();
                BlobClient blob = blobClient.GetBlobClient($"{newGuid}.json");

                await using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(myQueueItem ?? "")))
                {
                    await blob.UploadAsync(ms);
                }

            }
            catch (Exception ex)
            {
                log.LogInformation($"C# ServiceBus Error: {ex.Message}");
                using (var httpClient = new HttpClient())
                {
                    await httpClient.PostAsync(Environment.GetEnvironmentVariable("LogicAppUrl"), new StringContent(myQueueItem, Encoding.UTF8, "application/json"));
                }
            }
        }
    }
}
