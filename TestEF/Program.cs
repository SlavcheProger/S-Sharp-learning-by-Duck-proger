using Newtonsoft.Json;
using System;
using System.IO;
using TestEF.Controllers;

namespace TestEF
{
    public class Program
    {
        public static DataBase DB;
        static void Main(string[] args)
        {
            File.Delete(@"..\..\Log.txt");
            try
            {
                DB = DataBaseController.LoadDB(DB);
                AppController.ProgExecute(DB);
            }
            catch (Exception exception)
            {
                Log.ConsoleLog(ConsoleColor.Green, exception.ToString());
            }
            finally
            {
                Console.Write("Press any key to quit");
                Console.ReadKey();
            }
        }
    }
}

// добавить метод по поиску конкретного item'а
// связаться с МС по поводу Ripottai