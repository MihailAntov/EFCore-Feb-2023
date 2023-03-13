using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Import
{
    public class UserDto
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("age")]
        public int? Age { get; set; }
    }
}
