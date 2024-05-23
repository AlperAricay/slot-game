using System;
using Newtonsoft.Json;

namespace Gameplay.SerializationModule
{
    public class NewtonsoftJsonSerializationService : ISerializationService
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
            { NullValueHandling = NullValueHandling.Ignore };
        
        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, SerializerSettings);
        }

        public T Deserialize<T>(string dataAsString)
        {
            return JsonConvert.DeserializeObject<T>(dataAsString, SerializerSettings);
        }

        public object Deserialize(Type type, string dataAsString)
        {
            return JsonConvert.DeserializeObject(dataAsString, type, SerializerSettings);
        }
    }
}