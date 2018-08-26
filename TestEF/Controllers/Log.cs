using System;
using System.IO;

namespace TestEF.Controllers
{
    public class Log
    {
        public static void ConsoleLog(ConsoleColor clr, string message)
        {
            Console.ForegroundColor = clr;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void WriteToLog(String error )
        {
            File.AppendAllText(@"..\..\Log.txt", error);
        }
    }
}
