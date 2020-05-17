using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TataBot.Common.Models.Zamzar
{
    public class ZamzarJobResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("sandbox")]
        public bool Sandbox { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("finished_at ")]
        public object FinishedAt { get; set; }
        [JsonProperty("source_file")]
        public SourceFileModel SourceFile { get; set; }
        [JsonProperty("target_files")]
        public List<SourceFileModel> TargetFiles { get; set; }
        [JsonProperty("target_format")]
        public string TargetFormat { get; set; }
        [JsonProperty("credit_cost")]
        public int CreditCost { get; set; }
    }
}