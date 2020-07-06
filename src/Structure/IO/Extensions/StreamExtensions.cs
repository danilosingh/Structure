using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Structure.IO.Extensions
{
    public static class StreamExtensions
    {
        public static void SerializeTo<T>(this T o, Stream stream)
        {
            new BinaryFormatter().Serialize(stream, o); 
        }

        public static T Deserialize<T>(this Stream stream)
        {
            return (T)new BinaryFormatter().Deserialize(stream);
        }
    }
}
