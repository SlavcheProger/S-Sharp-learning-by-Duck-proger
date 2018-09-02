using System;
using static TestEF.Controllers.AppController;

namespace TestEF.Controllers
{
    class Misc
    {
        public static int CreateRandomId(Item itemType, DataBase DB)
        {
            var rand = new Random();
            var id = rand.Next(0, 1000);
            if (itemType == Item.car) // switch-case
            {
                while (DB.Cars.Exists(x => x.Id == id))
                {
                    id = rand.Next(0, 1000);
                }
            }
            else if (itemType == Item.plane)
            {
                while (DB.Planes.Exists(x => x.Id == id))
                {
                    id = rand.Next(0, 1000);
                }
            }
            return id;
        }

        public static int ChoiseHandler()
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

        public static void TestOnInput(Item itemType, DataBase DB) // передавать енам значение
        {
            Log.ConsoleLog(ConsoleColor.Yellow, $"Insert {itemType}`s id, or type \"exit\" to cancel");
            var input = Console.ReadLine();
            if (input != "exit")
            {
                var id = Convert.ToInt32(input);
                if (itemType == Item.car)
                {
                    var indexOfTheItem = DB.Cars.FindIndex(x => x.Id == id);
                    DB.Cars.RemoveAt(indexOfTheItem);
                }
                else if (itemType == Item.plane)
                {
                    var IndexOfTheItem = DB.Planes.FindIndex(x => x.Id == id);
                    DB.Planes.RemoveAt(IndexOfTheItem);
                }
            }
        }
    }
}
