using System.IO;
using System.IO.Compression;

namespace Structure.IO.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static ZipArchiveEntry CreateEntryFromText(this ZipArchive zip, string entryName, string text)
        {
            var entry = zip.CreateEntry(entryName);

            using (var entryStream = entry.Open())
            using (var streamWriter = new StreamWriter(entryStream))
            {
                streamWriter.Write(text);
            }

            return entry;
        }
    }
}
