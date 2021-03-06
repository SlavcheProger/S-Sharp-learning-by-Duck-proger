﻿using System;
using System.IO;

namespace TestEF.Controllers
{
    public class Log
    {
        public static void ConsoleLog(ConsoleColor clr, string message)
        {
            Console.ForegroundColor = clr;
            Console.WriteLine($"{message} \n"); // лучше сделать так вместо строчки с выводом пустой строки
            Console.ResetColor();
        }
        public static void WriteDataToFile(string filepath, string message)
        {
            File.AppendAllText(filepath, message);
        }
    }
}
