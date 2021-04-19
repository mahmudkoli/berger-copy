using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Extensions
{
    public static class LoggerExtension
    {
        //public static void ToTextFileLog(string message)
        //{
        //    var startupPath = Directory.GetCurrentDirectory();

        //    ToTextFileLog(message, startupPath);
        //}

        public static void ToTextFileLog(string message, string startupPath, string folderName = "Logs", string subfolderName = "Worker", string fileName = "ErrorLog", string extention = ".txt")
        {
            var toDay = DateTime.Now.ToString("yyyy-MM-dd");
            var dir = Path.Combine(startupPath, folderName, subfolderName);
            var filePath = Path.Combine(dir, fileName + "_" + toDay + extention);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var msg = message + Environment.NewLine;
            File.AppendAllText(filePath, msg);
        }

        public static void ToTextFileLog(string message, string path, string fileName)
        {
            var filePath = Path.Combine(path, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var msg = message + Environment.NewLine;
            File.AppendAllText(filePath, msg);
        }

        public static void ToWriteLog(string message, string startupPath)
        {
            ToTextFileLog(message, startupPath);
        }
        public static void ToWriteHttpLog(string message, string path)
        {
            string fileName = $"httpLogFile-{DateTime.Now.ToString("yyyyMMdd")}.txt";
            ToTextFileLog(message, path, fileName);
        }
    }
}
