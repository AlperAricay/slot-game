using System;

namespace Gameplay.SerializationModule
{
    public interface ISerializationService
    {
        string Serialize<T>(T data);

        T Deserialize<T>(string dataAsString);
        object Deserialize(Type type,string dataAsString);
    }
}