using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Structure.Helpers
{
    public static class ExplorerHelper
    {
        public static void AbrirExplorer(string pathArquivo = null)
        {
            string argumento = string.IsNullOrEmpty(pathArquivo) ? string.Empty : @"/select, " + pathArquivo;

            Process.Start("explorer.exe", argumento);
        }

        public static string GetPathTemporario(string nomeArquivo = null)
        {
            return Path.GetTempPath() + nomeArquivo;
        }
    }
}
