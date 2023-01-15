using Newtonsoft.Json;

namespace Rentire.Data.Utils
{
    public static class RJsonParser
    {
        public static string SerializeJson<T>(T data)
        {
            var serializedObj = JsonConvert.SerializeObject(data, 
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Include
                });
            return serializedObj;
        }

        public static T DeserializeJson<T>(string json)
        {
            var deserializedObj = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
            });
            return deserializedObj;
        }
    }
}