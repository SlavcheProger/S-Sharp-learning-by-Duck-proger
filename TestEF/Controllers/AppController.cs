using System;
using System.Linq;

namespace TestEF.Controllers
{
    class AppController
    {
        public enum Item { car = 1, plane = 2 }; // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
                var TransportType = (Item)Enum.Parse(typeof(Item), itemInput); //переменные именуем с маленькой буквы

                Log.ConsoleLog(ConsoleColor.Yellow, $"1-remove {TransportType} (by ID)\n2-add default {TransportType}" +
                    $" \n3-add {TransportType} with parameters \n4-search info about the {TransportType} in DataBase");

                var choice = Misc.ChoiceHandler();

                var id = 1;  //Misc.FindId(TransportType); - не понимаю смысла в этом методе - объясни мне.  + пока ставлю тут костыль - потом надо будет исправить

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
                    RemoveItemById(TransportType, db); // по названи. понятно, что надо передавать ID
                    break;
                case 2:
                    AddNewDefaultItem(id, TransportType, db); // вот тут id не нужен
                    break;
                case 3:
                    AddNewCustomItem(id, TransportType, db); // тоже id не нужен - entity framework сам генерит id
                    break;
                case 4:
                    GetItemById(id, TransportType, db);
                    break;
            }
            db.SaveChanges(); // не забывай сохранять изменения в бд, иначе ничего в ней не изменится!!!!!!!!!!!!!
            //DataBaseController.WriteDataToFile(db); изза этой строчки летели ошибки. Ты теперь работаешь с нормальной БД. Теперь не надо сохранять в файл. Но, если интересна сама ошибка, то он не мог преобразовать нашу новую бд в DataBase (который мы использовали раньше) 
        }

        public static void AddNewCustomItem(int id, Item TransportType, TransportContext db) // id не нужно передавать
        {
            retry:
            try
            {
                Console.Write("Speed: ");
                var par1 = Convert.ToInt32(Console.ReadLine()); // один и тот же код фиг знает сколько раз - вынеси в метод и вызывай его.
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
                    var ppar2 = Convert.ToInt32(Console.ReadLine()); // аналогично
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

        public static void AddNewDefaultItem(int id, Item TransportType, TransportContext db) // передавать енам значение  + Я говорил, что Entity framework сам генерит id. Тебе не надо самому его передавать
        {
            if (TransportType == Item.car) // переделать на switch-case
            {
                db.Cars.Add(new Car());
            }
            else if (TransportType == Item.plane)
                db.Planes.Add(new Plane());
        }

        public static void RemoveItemById(Item TransportType, TransportContext db) // передавать енам значение + ГДЕ УДАЛЕНИЕ??? Метод называется "Удалить объект по ID" + Почему ты не передаешь ID?
        {
            retry:
            try
            {
                if ((TransportType == Item.car && db.Cars.Count() != 0) || (TransportType == Item.plane && db.Planes.Count() != 0))
                {
                    Misc.TestOnInput(TransportType, db); // здесь ты должен был уже удалить объект. + почитай в этом методе коменты
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
                        foreach (var item in db.Cars) // по
                        {
                            var testItem = db.Cars.Find(id); // так ты можешь найти нужный тебе объект по id. Не надо вручную искать - переделай везде на такое.

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
