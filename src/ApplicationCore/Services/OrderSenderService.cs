using System.Net.Http;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.ApplicationCore.Services;
public class OrderSenderService : IOrderSenderService
{
    private readonly HttpClient _httpClient;
    private readonly string _serviceBusConnectionString;
    private readonly string _queueName;
    private readonly string _orderDeliveryUrl;

    public OrderSenderService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _serviceBusConnectionString = configuration["ServiceBusConnectionString"];
        _queueName = configuration["QueueName"];
        _orderDeliveryUrl = configuration["OrderDeliveryUrl"];
    }

    public async Task Process(Order order)
    {
        var orderMessage = JsonConvert.SerializeObject(order);
        await _httpClient.PostAsync(_orderDeliveryUrl, new StringContent(orderMessage));
        await SendToServiceBus(orderMessage);
    }

    private async Task SendToServiceBus(string orderMessage)
    {
        await using (var client = new ServiceBusClient(_serviceBusConnectionString))
        {
            await using (var sender = client.CreateSender(_queueName))
            {
                var message = new ServiceBusMessage(orderMessage);
                await sender.SendMessageAsync(message);
            }
        }
    }
}
