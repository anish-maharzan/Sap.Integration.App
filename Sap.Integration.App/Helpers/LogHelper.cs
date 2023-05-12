using System;
using System.IO;

namespace Sap.Integration.App.Helpers
{
    public class LogHelper
    {
        /* LOG TRACKER */
        public static void WriteLog(string messageString)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLog\\";
                if (!Directory.Exists(appPath))
                {
                    Directory.CreateDirectory(appPath);
                }

                string filePath = string.Format("{0}\\{1}.txt", appPath, DateTime.Now.ToString("dd-MM-yyyy"));
                if (!File.Exists(filePath))
                {
                    FileStream fileStream = default(FileStream);
                    fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    fileStream.Close();
                }

                StreamWriter streamWriter = new StreamWriter(filePath, true);
                streamWriter.WriteLine(string.Format("{0} : {1}", DateTime.Now.ToString(), messageString));
                streamWriter.WriteLine("--------------------------------------------------------");
                streamWriter.Close();
            }
            catch (Exception) { }
        }

    }
}
