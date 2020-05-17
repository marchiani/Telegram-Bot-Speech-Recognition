using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TataBot.Common.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public static string GetJson(object model)
        {
            return JsonConvert.SerializeObject(model, Settings);
        }

        public static T GetObject<T>(string model)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(model, Settings);
        }
    }
}