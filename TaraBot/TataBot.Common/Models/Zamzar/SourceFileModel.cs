using Newtonsoft.Json;

namespace TataBot.Common.Models.Zamzar
{
    public class SourceFileModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
    }
}