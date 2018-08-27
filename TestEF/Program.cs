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
                DB = DataBaseController.LoadDB(DB); //зачем тебе тут передавть бд, если этот метод считывает и возвращает бд из файла?
                AppController.ProgExecute(DB);
            }
            catch (Exception exception)
            {
                Log.ConsoleLog(ConsoleColor.Green, exception.ToString());
            }
            finally
            {
                Console.Write("Press any key to quit: ");
                Console.ReadKey();
            }
        }
    }
}

// мелкие правки в коде
// прочитать про enum, добавить enum TransportType, в методы, которые требуют тип (car/plane) передавать не строку, а enum значение
// метод поиска я переделаю - потом запушу и объясню, как оно работает


// связаться с МС по поводу Ripottai