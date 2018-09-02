using System;
using System.Linq;

namespace TestEF.Controllers
{
    class AppController
    {
        public enum Item {car = 1, plane = 2};
        public static void ProgExecute(DataBase DB)
        {
            while (true)
            {
                Log.ConsoleLog(ConsoleColor.Yellow, "what do you want to change: car or plane?");
                tryAgain:
                var itemInput = Console.ReadLine().ToLower();
                if (itemInput != "car" && itemInput != "plane")
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, "Please, enter \"car\" or \"plane\"");
                    goto tryAgain;
                }
                Item itemType = (Item)Enum.Parse(typeof(Item), itemInput);

                Log.ConsoleLog(ConsoleColor.Yellow, $"1-remove {itemType} (by ID)\n2-add default {itemType}" +
                    $" \n3-add {itemType} with parameters \n4-search info about the {itemType} in DataBase");

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

        public static void CommandHandler(int id, Item itemType, int choice, DataBase DB) // передавать енам значение
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
                case 4:
                    SearchForItem(id, itemType, DB);
                    break;
            }
            DataBaseController.WriteDataToFile(DB);
        }

        public static void AddNewCustomItem(int id, Item itemType, DataBase DB) // передавать енам значение
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
                if (itemType == Item.car) // переделать на switch-case
                {
                    Console.Write("Model: ");
                    var cpar1 = Console.ReadLine();
                    Console.Write("Color: ");
                    var cpar2 = Console.ReadLine();
                    DB.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, id));
                }
                else if (itemType == Item.plane)
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

        public static void AddNewDefaultItem(int id, Item itemType, DataBase DB) // передавать енам значение
        {
            if (itemType == Item.car) // переделать на switch-case
            {
                DB.Cars.Add(new Car(id));
            }
            else if (itemType == Item.plane)
                DB.Planes.Add(new Plane(id));
        }

        public static void RemoveItemById(Item itemType, DataBase DB) // передавать енам значение
        {
        retry:
            try
            {
                if ((itemType == Item.car && DB.Cars.Count != 0) || (itemType == Item.plane && DB.Planes.Count != 0))
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

        public static void SearchForItem(int id, Item itemType, DataBase DB) // передавать енам значение
                                                                               // переименовать в GetItemById
        {
            if (itemType == Item.car && DB.Cars.Exists(x => x.Id == id)) // switch-case
            {
                foreach (var item in DB.Cars)
                {
                    if (item.Id == id)
                    {
                        Log.ConsoleLog(ConsoleColor.Cyan, $"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nColor: {item.Color} \nModel: {item.Model} \nId: {item.Id}\ncar\n*");
                    }
                }
            }
            else if (itemType == Item.plane && DB.Planes.Exists(x => x.Id == id))
            {
                foreach (var item in DB.Planes)
                {
                    if (item.Id == id)
                    {
                        Log.ConsoleLog(ConsoleColor.Red, $"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nAvia company: {item.AviaComp} \nId: {item.Id}\nplane\n*");
                    }
                }
            }
        }
    }
}
