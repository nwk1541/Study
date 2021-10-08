using System;
using System.IO;
using System.Text;

namespace DataConvert
{
    public static class Util
    {
        public static void ReportCode(Enums.ErrorCode code, string desc = null)
        {
            string result = string.Format("[ErrorCode]:{0}", code);

            if (desc != null)
                result += string.Format(" {0}", desc);

            ConsoleText(result);
        }

        public static void ConsoleText(string text)
        {
            Console.WriteLine(text);
        }

        public static void ConsoleText(string text, params object[] args)
        {
            Console.WriteLine(text, args);
        }

        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public static bool IsExistDirectory(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            return dirInfo != null ? dirInfo.Exists : false;
        }

        public static DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}
