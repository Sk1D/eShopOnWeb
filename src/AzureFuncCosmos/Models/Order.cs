using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureFuncCosmos.Models;
public class Order
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("BuyerId")]
    public string BuyerId { get; set; }

    [JsonProperty("OrderDate")]
    public DateTimeOffset OrderDate { get; set; }

    [JsonProperty("ShipToAddress")]
    public Address ShipToAddress { get; set; }

    [JsonProperty("OrderItems")]
    public List<OrderItem> OrderItems { get; set; }
}
