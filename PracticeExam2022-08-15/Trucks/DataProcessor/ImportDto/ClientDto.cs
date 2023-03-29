using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ImportDto
{

    [JsonObject]
    public class ClientDto
    {
        [JsonProperty("Name")]
        [MaxLength(40),MinLength(3)]
        [Required]
        public string Name { get; set; } = null!;

        [JsonProperty("Nationality")]
        [MaxLength(40),MinLength(2)]
        [Required]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; } = null!;


        public int[] Trucks { get; set; }

        //    "Name": "DHL SERVICES LIMITED",
        //"Nationality": "The United Kingdom",
        //"Type": "golden",
        //"Trucks": [
        //  4,
        //  17,
        //  17,
        //  98
        //]
    }
}
