using System;
using static TestEF.Controllers.AppController;
using System.Linq;

namespace TestEF.Controllers
{
    class Misc
    {
        public static int FindId(Item TransportType) // Что делает этот метод? Зачем он?
        {
            int id;
            switch (TransportType)
            {
                default:
                case Item.car:
                    id = db.Cars.Count();
                    break;
                case Item.plane:
                    id = db.Planes.Count();
                    break;
            }

            return id;
        }

        public static int ChoiceHandler()  // "handler" - обработчик. Тут ты выбираешь действие, а не обрабатываешь. ПЕРЕИМЕНОВАТЬ метод!
        {
        theChoice:

            var result = Int32.TryParse(Console.ReadLine(), out int choice);
            if (!result || choice != 1 && choice != 2 && choice != 3 && choice != 4)
            {
                Log.ConsoleLog(ConsoleColor.Yellow, "Choose 1, 2, 3 or 4");
                goto theChoice;
            }
            return choice;
        }

        public static void TestOnInput(Item TransportType, TransportContext DB) // почему метод "проверка на входные данные" занимается удалением объектов? 
        {
            Log.ConsoleLog(ConsoleColor.Yellow, $"Insert {TransportType}`s id, or type \"exit\" to cancel");
            var input = Console.ReadLine();
            if (input != "exit")
            {
                var id = Convert.ToInt32(input); //ты заправшиваешь id в методе ProgExecute. Зачем щас второй раз?  И к тому же, нет обертки try-catch на неверное преобразование типов
                switch (TransportType)
                {
                    case Item.car:
                        var CarForDeletion = db.Cars.Find(id); // переменные именуем с маленькой буквы.ну и названия делаем с использованием переводчика

                        db.Cars.Remove(CarForDeletion);

                        db.SaveChanges(); // первый раз

                        break;
                    case Item.plane:
                        var PlaneForDeletion = db.Planes.Find(id); // тоже самое

                        db.Planes.Remove(PlaneForDeletion);

                        db.SaveChanges(); // второй раз.... сотый раз... а не проще вынести из try-catch и тогда надо будет писать один раз?
                        break;
                }
            }
        }
    }
}
