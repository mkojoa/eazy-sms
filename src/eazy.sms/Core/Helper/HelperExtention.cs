using System;
using System.IO;
using System.Threading.Tasks;

namespace eazy.sms.Core.Helper
{
    public static class HelperExtention
    {
        public static async Task<string> LoadTextFile(string filePath)
        {
            var reader = new StreamReader(filePath);
            var content = await reader.ReadToEndAsync();
            reader.Close();
            return content;
        }

        internal static void CreateDirectoryIfDoesNotExist(string folder)
        {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        }

        internal static void CreateFileIfDoesNotExist(string file)
        {
            if (!File.Exists(file)) File.Create(file).Close();
        }

        internal static bool CheckIfExistFile(string file)
        {
            return File.Exists(file);
        }

        public static void LogWrite(string logMessage)
        {
            var appDir = Directory.GetCurrentDirectory() + "\\Logs";
            CreateDirectoryIfDoesNotExist(appDir);
            CreateFileIfDoesNotExist(appDir + "\\log.txt");
            try
            {
                using var w = File.AppendText(appDir + "\\log.txt");
                Log(logMessage, w);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}