using System;
using System.IO;

namespace Gameplay.SerializationModule
{
    public class FileSerializationController
    {
        private readonly ISerializationService _serializationService;

        public FileSerializationController(ISerializationService serializationService)
        {
            _serializationService = serializationService;
        }

        public void SerializeToFile<T>(T data, string filePath)
        {
            var dataAsString = _serializationService.Serialize(data);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            
            File.WriteAllText(filePath, dataAsString);
        }

        public T DeserializeFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            var dataAsString = File.ReadAllText(filePath);

            var data = _serializationService.Deserialize<T>(dataAsString);
            return data;
        }

        public object DeserializeFromFile(Type type, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            var dataAsString = File.ReadAllText(filePath);

            var data = _serializationService.Deserialize(type, dataAsString);
            return data;
        }
    }
}