using System;

namespace TestEF.Controllers
{
    class Misc
    {
        public static int CreateRandomId(string itemType, DataBase DB)
        {
            var rand = new Random();
            var id = rand.Next(0, 1000);
            if (itemType == "car")
            {
                while (DB.Cars.Exists(x => x.Id == id))
                {
                    id = rand.Next(0, 1000);
                }
            }
            else if (itemType == "plane")
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
            if (!result || choice != 1 && choice != 2 && choice != 3)
            {
                Log.ConsoleLog(ConsoleColor.Yellow, "Choose 1, 2 or 3");
                goto theChoice;
            }
            return choice;
        }

        public static void TestOnInput(string itemType, DataBase DB)
        {
            Log.ConsoleLog(ConsoleColor.Yellow, $"Insert {itemType}`s id, or type \"exit\" to cancel");
            var input = Console.ReadLine();
            if (input != "exit")
            {
                var id = Convert.ToInt32(input);
                if (itemType == "car")
                {
                    var IndexOfTheItem = DB.Cars.FindIndex(x => x.Id == id); //переменные называем с маленькой буквы lowerCamelCase'ом
                    DB.Cars.RemoveAt(IndexOfTheItem);
                }
                else if (itemType == "plane")
                {
                    var IndexOfTheItem = DB.Planes.FindIndex(x => x.Id == id);
                    DB.Planes.RemoveAt(IndexOfTheItem);
                }
            }
        }
    }
}
