using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Structure.IO.Helpers
{
    public static class GZipHelper
    {
        public static string UnzipBase64String(string str)
        {
            var bytes = Convert.FromBase64String(str);
            return Unzip(bytes);
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}
