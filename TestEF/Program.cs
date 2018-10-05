using System;
using System.IO;
using TestEF.Controllers;

namespace TestEF
{
    public class Program
    {
        static void Main(string[] args)
        {
            File.Delete(@"..\..\Log.txt");

            try
            {
                AppController.ProgExecute();
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

// посмотреть видосик про unit tests и попробовать на тестовом проекте (хотя бы на тестовом классе)         /позже

// связаться с МС по поводу Ripottai                /позже