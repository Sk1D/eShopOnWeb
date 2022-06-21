using Newtonsoft.Json;

namespace AzureFuncCosmos.Models;
public class ItemOrdered
{
    [JsonProperty("CatalogItemId")]
    public int CatalogItemId { get; set; }

    [JsonProperty("ProductName")]
    public string ProductName { get; set; }

    [JsonProperty("PictureUri")]
    public string PictureUri { get; set; }
}
