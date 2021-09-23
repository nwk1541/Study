using System;
using System.Text;

namespace DataConvert
{
    public static class Util
    {
        public static void ConsoleText(string text)
        {
            Console.WriteLine(text);
        }

        public static void ConsoleText(string text, params object[] args)
        {
            Console.WriteLine(text, args);
        }
    }
}
