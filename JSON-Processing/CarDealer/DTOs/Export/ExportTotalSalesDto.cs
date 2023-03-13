using Newtonsoft.Json;
using CarDealer.Models;

namespace CarDealer.DTOs.Export
{
    public class ExportTotalSalesDto
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }
        [JsonProperty("spentMoney")]

        public decimal SpentMoney { get; set; }
        
    }
}
