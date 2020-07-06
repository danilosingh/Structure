using System.IO;
using System.Text;

namespace Structure.Helpers
{
    public static class StreamHelper
    {
        public static MemoryStream LoadStringToStream(string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, str.Length, true);
            writer.Write(str);
            writer.Flush();
            writer.Close();
            stream.Position = 0;
            return stream;
        }

        public static Stream LoadFileStream(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static void SaveStreamToFile(Stream stream, string fileName)
        {
            using (Stream file = File.Create(fileName))
            {
                CopyStream(stream, file);
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public static string ToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
