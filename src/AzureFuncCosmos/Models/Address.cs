using Newtonsoft.Json;

namespace AzureFuncCosmos.Models;
public class Address
{
    [JsonProperty("Street")]
    public string Street { get; set; }

    [JsonProperty("City")]
    public string City { get; set; }

    [JsonProperty("State")]
    public string State { get; set; }

    [JsonProperty("Country")]
    public string Country { get; set; }

    [JsonProperty("ZipCode")]
    public string ZipCode { get; set; }
}
