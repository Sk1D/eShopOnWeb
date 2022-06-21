using System;
using System.IO;
using System.Threading.Tasks;
using AzureFuncCosmos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFuncCosmos;
public static class DeliveryOrderProcessor
{
    [FunctionName("DeliveryOrderProcessor")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        // just create Azure Cosmos DB account -> Keys
        log.LogInformation("C# HTTP trigger function processed a request.");
        var endpointUrl = Environment.GetEnvironmentVariable("CosmosDbEndpointUrl");
        var authorizationKey = Environment.GetEnvironmentVariable("CosmosDbAuthorizationKey");
        var databaseId = Environment.GetEnvironmentVariable("CosmosDbName");
        var containerId = Environment.GetEnvironmentVariable("CosmosDbContainer");

        using (var cosmosClient = new CosmosClient(endpointUrl, authorizationKey))
        {
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Container container = await cosmosClient.GetDatabase(databaseId).CreateContainerIfNotExistsAsync(containerId, "/CreatedDate");
            using (var stream = new StreamReader(req.Body))
            {
                string requestBody = await stream.ReadToEndAsync();

                log.LogInformation($"string from body - {requestBody}");
                var order = JsonConvert.DeserializeObject<Order>(requestBody);
                log.LogInformation($"order.Id - {order.Id}");
                await container.CreateItemAsync(order);

                return new OkObjectResult($"order {order.Id} uploaded successfully");
            }
        }
    }
}
