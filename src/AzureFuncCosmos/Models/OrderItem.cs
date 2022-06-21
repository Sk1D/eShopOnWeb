using Newtonsoft.Json;

namespace AzureFuncCosmos.Models;
public class OrderItem
{
    [JsonProperty("Id")]
    public int Id { get; set; }

    [JsonProperty("ItemOrdered")]
    public ItemOrdered ItemOrdered { get; set; }

    [JsonProperty("UnitPrice")]
    public decimal UnitPrice { get; set; }

    [JsonProperty("Units")]
    public int Units { get; set; }

}
