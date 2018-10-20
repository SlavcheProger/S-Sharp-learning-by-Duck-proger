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
                Log.ConsoleLog(ConsoleColor.Yellow, "what do you want to change?\n1-car\n2-plane");
                tryAgain:
                var itemInput = Console.ReadLine();
                if (itemInput != "1" && itemInput != "2")
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, "Please, choose \"car\" or \"plane\"");
                    goto tryAgain;
                }
                var transportType = (TransportType)Enum.Parse(typeof(TransportType), itemInput);

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

        public static void CommandHandler(TransportType transportType, int choice, TransportContext db)
        {
            switch (choice)
            {
                case 1:
                    var id = Misc.GetId(transportType, db);
                    RemoveItemById(transportType, db, id);
                    break;
                case 2:
                    AddNewDefaultItem(transportType, db);
                    break;
                case 3:
                    AddNewCustomItem(transportType, db);
                    break;
                case 4:
                    id = Misc.GetId(transportType, db);
                    GetItemById(transportType, db, id);
                    break;
            }
            db.SaveChanges();
        }

        public static void AddNewCustomItem(TransportType transportType, TransportContext db)
        {
            retry:
            try
            {
                Console.Write("Speed: ");
                var par1 = Convert.ToInt32(Console.ReadLine()); // один и тот же код фиг знает сколько раз - вынеси в метод и вызывай его.          //А что именно выводить в другой метод?  // класс program пункт 3
                Console.Write("Fuel consumation: ");
                var par2 = Convert.ToDouble(Console.ReadLine());
                Console.Write("Cost of maintaining: ");
                var par3 = Convert.ToInt32(Console.ReadLine());
                switch (transportType)
                {
                    case TransportType.car:
                        Console.Write("Model: ");
                        var cpar1 = Console.ReadLine();
                        Console.Write("Color: ");
                        var cpar2 = Console.ReadLine();
                        db.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, 1));
                        break;

                    case TransportType.plane:
                        var ppar1 = Console.ReadLine();
                        Console.Write("Amount of turbines: ");
                        var ppar2 = Convert.ToInt32(Console.ReadLine());
                        db.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, 1));
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.WriteDataToFile(@"..\..\Log.txt",exception.ToString());
                Log.ConsoleLog(ConsoleColor.Yellow, "Incorrect format of the last variable, try again");
                goto retry;
            }
        }

        public static void AddNewDefaultItem(TransportType transportType, TransportContext db)
        {
            switch (transportType)
            {
                case TransportType.car:
                    db.Cars.Add(new Car());
                    break;
                case TransportType.plane:
                    db.Planes.Add(new Plane());
                    break;
            }
        }

        public static void RemoveItemById(TransportType transportType, TransportContext db, int id)
        {
            retry:
            try
            {
                if ((transportType == TransportType.car && db.Cars.Count() != 0) || (transportType == TransportType.plane && db.Planes.Count() != 0))
                {
                    switch (transportType)
                    {
                        case TransportType.car:
                            var carForDeletion = db.Cars.Find(id); // у тебя есть метод GetItemById. зачем он? я думаю, что именно для этого

                            db.Cars.Remove(carForDeletion);

                            break;
                        case TransportType.plane:
                            var planeForDeletion = db.Planes.Find(id);  // у тебя есть метод GetItemById. зачем он? я думаю, что именно для этого

                            db.Planes.Remove(planeForDeletion);

                            break;
                    }
                    db.SaveChanges();
                }
                else
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, $"Sorry, there is no {transportType} in DataBase.");
                }
            }
            catch (Exception exception)
            {
                Log.WriteDataToFile(@"..\..\Log.txt", exception.ToString());
                Log.ConsoleLog(ConsoleColor.Green, exception.ToString());
                goto retry;
            }
        }

        public static void GetItemById(TransportType transportType, TransportContext db, int id)
        {
            reEnter:
            try
            {
                switch (transportType)
                {
                    case TransportType.car:
                        var carItem = db.Cars.Find(id); 
                        Log.ConsoleLog(ConsoleColor.Cyan, $"Speed: {carItem.Speed}\nCost of maintain: {carItem.CostOfMaintain} \nFuel consumation: {carItem.FuelConsum} \nColor: {carItem.Color} \nModel: {carItem.Model} \nId: {carItem.Id}\ncar\n*");

                        break;
                    case TransportType.plane:
                        var planeItem = db.Planes.Find(id); 
                        Log.ConsoleLog(ConsoleColor.Red, $"Speed: {planeItem.Speed}\nCost of maintain: {planeItem.CostOfMaintain} \nFuel consumation: {planeItem.FuelConsum} \nAvia company: {planeItem.AviaComp} \nId: {planeItem.Id}\nplane\n*");

                        break;
                }
            }
            catch (Exception)
            {
                goto reEnter;
            }
        } //метод должен возвращать объект, а не выводить на экран данные. ексли не можешь возвращать неизвестный тип, то сделай хотя бы несколько методов поиска (для машин и самолетов)
    }
}
