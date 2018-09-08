using System;
using System.Linq;

namespace TestEF.Controllers
{
    class AppController
    {
        public enum Item { car = 1, plane = 2 };
        public static TransportContext db;
        public static void ProgExecute()
        {

            db = DataBaseController.LoadDB();
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
                Item TransportType = (Item)Enum.Parse(typeof(Item), itemInput);

                Log.ConsoleLog(ConsoleColor.Yellow, $"1-remove {TransportType} (by ID)\n2-add default {TransportType}" +
                    $" \n3-add {TransportType} with parameters \n4-search info about the {TransportType} in DataBase");

                var choice = Misc.ChoiseHandler();

                var id = Misc.FindId(TransportType);

                CommandHandler(id, TransportType, choice, db);

                DataBaseController.ShowDB(db);
                Log.ConsoleLog(ConsoleColor.Yellow, "If you want to exit, type 0, or anything else to continue: ");

                if (Int32.TryParse(Console.ReadLine(), out int input) && input == 0)
                {
                    break;
                }
            }
        }

        public static void CommandHandler(int id, Item TransportType, int choice, TransportContext db)
        {
            switch (choice)
            {
                case 1:
                    RemoveItemById(TransportType, db);
                    break;
                case 2:
                    AddNewDefaultItem(id, TransportType, db);
                    break;
                case 3:
                    AddNewCustomItem(id, TransportType, db);
                    break;
                case 4:
                    GetItemById(id, TransportType, db);
                    break;
            }
            DataBaseController.WriteDataToFile(db);
        }

        public static void AddNewCustomItem(int id, Item TransportType, TransportContext db)
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
                if (TransportType == Item.car) // переделать на switch-case
                {
                    Console.Write("Model: ");
                    var cpar1 = Console.ReadLine();
                    Console.Write("Color: ");
                    var cpar2 = Console.ReadLine();
                    db.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, id));
                }
                else if (TransportType == Item.plane)
                {
                    var ppar1 = Console.ReadLine();
                    Console.Write("Amount of turbines: ");
                    var ppar2 = Convert.ToInt32(Console.ReadLine());
                    db.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, id));
                }
            }
            catch (Exception exception)
            {
                Log.WriteToLog(exception.ToString());
                Log.ConsoleLog(ConsoleColor.Yellow, "Incorrect format of the last variable, try again");
                goto retry;
            }
        }

        public static void AddNewDefaultItem(int id, Item TransportType, TransportContext db) // передавать енам значение
        {
            if (TransportType == Item.car) // переделать на switch-case
            {
                db.Cars.Add(new Car(id));
            }
            else if (TransportType == Item.plane)
                db.Planes.Add(new Plane(id));
        }

        public static void RemoveItemById(Item TransportType, TransportContext db) // передавать енам значение
        {
        retry:
            try
            {
                if ((TransportType == Item.car && db.Cars.Count() != 0) || (TransportType == Item.plane && db.Planes.Count() != 0))
                {
                    Misc.TestOnInput(TransportType, db);
                }
                else
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, $"Sorry, there is no {TransportType} in DataBase.");
                }
            }
            catch (Exception exception)
            {
                Log.WriteToLog(exception.ToString());
                Log.ConsoleLog(ConsoleColor.Yellow, $"Wrong {TransportType}`s id! Please, try again.");
                goto retry;
            }
        }

        public static void GetItemById(int id, Item TransportType, TransportContext db)
        {
        reEnter:
            try
            {
                switch (TransportType)
                {
                    case Item.car:
                        foreach (var item in db.Cars)
                        {
                            if (item.Id == id)
                            {
                                Log.ConsoleLog(ConsoleColor.Cyan, $"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nColor: {item.Color} \nModel: {item.Model} \nId: {item.Id}\ncar\n*");
                            }
                        }

                        break;
                    case Item.plane:
                        foreach (var item in db.Planes)
                        {
                            if (item.Id == id)
                            {
                                Log.ConsoleLog(ConsoleColor.Red, $"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nAvia company: {item.AviaComp} \nId: {item.Id}\nplane\n*");
                            }
                        }

                        break;
                }
            }
            catch (Exception)
            {

                goto reEnter;
            }
        }
    }
}
