using System;
using System.Linq;

namespace TestEF.Controllers
{
    class AppController
    {
        public static void ProgExecute(DataBase DB)
        {
            while (true)
            {
                Log.ConsoleLog(ConsoleColor.Yellow, "what do you want to change: car or plane?");
            tryAgain:
                var itemType = Console.ReadLine().ToLower();
                if (itemType != "car" && itemType != "plane")
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, "Please, enter \"car\" or \"plane\"");
                    goto tryAgain;
                }

                Log.ConsoleLog(ConsoleColor.Yellow, $"1-remove {itemType} (by ID)\n2-add default {itemType} \n3-add {itemType} with parameters");

                var choice = Misc.ChoiseHandler();

                var id = Misc.CreateRandomId(itemType, DB);

                CommandHandler(id, itemType, choice, DB);

                DataBaseController.ShowDB(DB);
                Log.ConsoleLog(ConsoleColor.Yellow, "If you want to exit, type 0, or anything else to continue: ");

                if (Int32.TryParse(Console.ReadLine(), out int input) && input == 0)
                {
                    break;
                }
            }
        }

        public static void CommandHandler(int id, string itemType, int choice, DataBase DB)
        {
            switch (choice)
            {
                case 1:
                    RemoveItemById(itemType, DB);
                    break;
                case 2:
                    AddNewDefaultItem(id, itemType, DB);
                    break;
                case 3:
                    AddNewCustomItem(id, itemType, DB);
                    break;
            }
            DataBaseController.WriteDataToFile(DB);
        }

        public static void AddNewCustomItem(int id, string itemType, DataBase DB)
        {
        retry:
            try
            {
                Console.Write("Speed: ");
                var par1 = Convert.ToInt32(Console.ReadLine());
                Console.Write("Fuel consumation: ");
                var par2 = Convert.ToDouble(Console.ReadLine());
                Console.Write("Cost of maintaining: ");
                var par3 = Convert.ToInt32(Console.ReadLine());
                if (itemType == "car")
                {
                    Console.Write("Model: ");
                    var cpar1 = Console.ReadLine();
                    Console.Write("Color: ");
                    var cpar2 = Console.ReadLine();
                    DB.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, id));
                }
                else if (itemType == "plane")
                {
                    var ppar1 = Console.ReadLine();
                    Console.Write("Amount of turbines: ");
                    var ppar2 = Convert.ToInt32(Console.ReadLine());
                    DB.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, id));
                }
            }
            catch (Exception exception)
            {
                Log.WriteToLog(exception.ToString());
                Log.ConsoleLog(ConsoleColor.Yellow, "Incorrect format of the last variable, try again");
                goto retry;
            }
        }

        public static void AddNewDefaultItem(int id, string itemType, DataBase DB)
        {
            if (itemType == "car")
            {
                DB.Cars.Add(new Car(id));
            }
            else if (itemType == "plane")
                DB.Planes.Add(new Plane(id));
        }

        public static void RemoveItemById(string itemType, DataBase DB)
        {
        retry:
            try
            {
                if ((itemType == "car" && DB.Cars.Count != 0) || (itemType == "plane" && DB.Planes.Count != 0))
                {
                    Misc.TestOnInput(itemType, DB);
                }
                else
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, $"Sorry, there is no {itemType} in DataBase.");
                }
            }
            catch (Exception exception)
            {
                Log.WriteToLog(exception.ToString());
                Log.ConsoleLog(ConsoleColor.Yellow, $"Wrong {itemType}`s id! Please, try again.");
                goto retry;
            }
        }
    }
}
