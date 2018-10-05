using System;
using System.Linq;
using TestEF.DataTypes;

namespace TestEF.Controllers
{
    class AppController
    {
        public static TransportContext db;
        public static void ProgExecute()
        {
            db = DataBaseController.LoadDB();
            while (true)
            {
                DataBaseController.ShowDB(db);
                Log.ConsoleLog(ConsoleColor.Yellow, "what do you want to change: car or plane?");
            tryAgain:
                var itemInput = Console.ReadLine().ToLower();
                if (itemInput != "car" && itemInput != "plane")
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, "Please, enter \"car\" or \"plane\"");
                    goto tryAgain;
                }
                var transportType = (EnumItems.Item)Enum.Parse(typeof(EnumItems.Item), itemInput);

                Log.ConsoleLog(ConsoleColor.Yellow, $"1-remove {transportType} (by ID)\n2-add default {transportType}" +
                    $" \n3-add {transportType} with parameters \n4-search info about the {transportType} in DataBase");

                var choice = Misc.ChoiceOfAction();

                CommandHandler(transportType, choice, db);

                Log.ConsoleLog(ConsoleColor.Yellow, "If you want to exit, type 0, or anything else to continue: ");

                if (Int32.TryParse(Console.ReadLine(), out int input) && input == 0)
                {
                    break;
                }
            }
        }

        public static void CommandHandler(EnumItems.Item transportType, int choice, TransportContext db)
        {
            switch (choice)
            {
                case 1:
                    RemoveItemById(transportType, db);
                    break;
                case 2:
                    AddNewDefaultItem(transportType, db);
                    break;
                case 3:
                    AddNewCustomItem(transportType, db);
                    break;
                case 4:
                    GetItemById(transportType, db);
                    break;
            }
            db.SaveChanges();
        }

        public static void AddNewCustomItem(EnumItems.Item transportType, TransportContext db)
        {
        retry:
            try
            {
                Console.Write("Speed: ");
                var par1 = Convert.ToInt32(Console.ReadLine()); // один и тот же код фиг знает сколько раз - вынеси в метод и вызывай его.          //А что именно выводить в другой метод?
                Console.Write("Fuel consumation: ");
                var par2 = Convert.ToDouble(Console.ReadLine());
                Console.Write("Cost of maintaining: ");
                var par3 = Convert.ToInt32(Console.ReadLine());
                switch (transportType)
                {
                    case EnumItems.Item.car:
                        Console.Write("Model: ");
                        var cpar1 = Console.ReadLine();
                        Console.Write("Color: ");
                        var cpar2 = Console.ReadLine();
                        db.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, 1));
                        break;

                    case EnumItems.Item.plane:
                        var ppar1 = Console.ReadLine();
                        Console.Write("Amount of turbines: ");
                        var ppar2 = Convert.ToInt32(Console.ReadLine());
                        db.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, 1));
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.WriteToLog(exception.ToString());
                Log.ConsoleLog(ConsoleColor.Yellow, "Incorrect format of the last variable, try again");
                goto retry;
            }
        }

        public static void AddNewDefaultItem(EnumItems.Item transportType, TransportContext db)
        {
            switch (transportType)
            {
                case EnumItems.Item.car:
                    db.Cars.Add(new Car());
                    break;
                case EnumItems.Item.plane:
                    db.Planes.Add(new Plane());
                    break;
            }
        }

        public static void RemoveItemById(EnumItems.Item transportType, TransportContext db)
        {
        retry:
            try
            {
                if ((transportType == EnumItems.Item.car && db.Cars.Count() != 0) || (transportType == EnumItems.Item.plane && db.Planes.Count() != 0))
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, $"Insert {transportType}`s id, or type \"exit\" to cancel");
                    var input = Console.ReadLine();
                    if (input != "exit")
                    {
                        var id = Convert.ToInt32(input);
                        switch (transportType)
                        {
                            case EnumItems.Item.car:
                                var carForDeletion = db.Cars.Find(id);

                                db.Cars.Remove(carForDeletion);

                                break;
                            case EnumItems.Item.plane:
                                var planeForDeletion = db.Planes.Find(id);

                                db.Planes.Remove(planeForDeletion);

                                break;
                        }

                        db.SaveChanges();
                    }
                }
                else
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, $"Sorry, there is no {transportType} in DataBase.");
                }
            }
            catch (Exception exception)
            {
                Log.WriteToLog(exception.ToString());
                Log.ConsoleLog(ConsoleColor.Yellow, $"Wrong {transportType}`s id! Please, try again.");
                goto retry;
            }
        }

        public static void GetItemById(EnumItems.Item transportType, TransportContext db)
        {
        reEnter:
            try
            {
                Console.Write("Enter item's Id: ");
                var id = Convert.ToInt32(Console.ReadLine());
                switch (transportType)
                {
                    case EnumItems.Item.car:
                        foreach (var item in db.Cars)
                        {
                            var testItem = db.Cars.Find(id);

                            if (item.Id == id)
                            {
                                Log.ConsoleLog(ConsoleColor.Cyan, $"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nColor: {item.Color} \nModel: {item.Model} \nId: {item.Id}\ncar\n*");
                            }
                        }

                        break;
                    case EnumItems.Item.plane:
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
