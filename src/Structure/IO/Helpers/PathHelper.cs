using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.IO.Helpers
{
    public static class PathHelper
    {        
        public static bool IsValidPath(string path)
        {            
            return Path.IsPathRooted(path) && !Path.GetInvalidPathChars().Any(x => path.Contains(x));
        }

        public static bool PathWithValidCharacters(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            
            string pathAux = string.Empty;
            List<string> pastas = path.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < pastas.Count; i++)
            {
                pathAux += "\\" + pastas[i];
                
                if (!FolterNameIsValid(pastas[i]))
                {
                    return false;
                }
            }

            if (pathAux != path)
            {
                return false;
            }

       
            return !Path.GetInvalidPathChars().Any(x => path.Contains(x));
        }

        public static bool FolterNameIsValid(string folterName)
        {
            return !Path.GetInvalidFileNameChars().Any(c => folterName.Contains(c));
        }

        public static bool FileNameIsValid(string fileName)
        {
            return !Path.GetInvalidFileNameChars().Any(c => fileName.Contains(c));            
        }

        public static string GetPathRoot(string path)
        {
            return Path.GetPathRoot(path);
        }

        public static void CreatePath(string path)
        {
            var auxPath = Path.GetDirectoryName(path);

            if (!Directory.Exists(auxPath))
            {
                Directory.CreateDirectory(auxPath);
            }
        }
    }
}
