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

/*
1) не забывай чистить using'и (удалять те импорты либ, которые в данном классе не используются). я все почистил, так что \то на будущее
2) сейчас заметил, что ты лог файл не создаешь. он никуда не записывает данных. в проводнике не могу найти его. до сих пор не понимаю, почему он не выбрасывает ошибку, что файл не существует.
3) сделай наконец то универсальный ввод чиселки с консоли. сделай метод, который будет возвращать число, а уже дальше по коду ты будешь как нибудь обрабатывать. получение id через этот  же метод.     
4) enum'ы не надо оборачивать классом. поэтому исправь использование енама Items и назови енам TransportType
5) правки по коду
*/





// посмотреть видосик про unit tests и попробовать на тестовом проекте (хотя бы на тестовом классе)         /позже

// связаться с МС по поводу Ripottai                /позже