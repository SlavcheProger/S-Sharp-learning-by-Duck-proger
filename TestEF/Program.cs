using Newtonsoft.Json;
using System;
using System.IO;

namespace TestEF
{
    public class Program
    {
        public static DataBase DB;
        static void Main(string[] args)
        {
            try
            {
                DB = LoadDB();
                ProgExecute();
            }
            catch (Exception exception)
            {
                ConsoleLog(4, exception.ToString());
            }
        }
        // console color output
        #region
        public static void ConsoleLog(int clr, string str)
        {
            switch (clr)
            {
	        
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                break;
                case 4:
                     Console.ForegroundColor = ConsoleColor.Green;                    
                break;
	        }
            Console.WriteLine(str);
            Console.ResetColor();
            Console.WriteLine();
        }
        ///////////////////////////////////////
        #endregion
        /// operating with DataBase File
        #region
        public static DataBase LoadDB()
        {
            try
            {
                DB = ReadDB();
                ShowDB(DB);
            }
            catch (Exception exception)
            {
                ConsoleLog(4, exception.ToString());
                if (!File.Exists(@"..\..\DB\DataBase.json"))
                {
                    ConsoleLog(1, "Data Base missing, creating a new one");
                    using (var fs = File.Create(@"..\..\DB\DataBase.json")) { }
                    DB = new DataBase();
                }
            }
            return DB;
        }

        public static DataBase ReadDB()
        {
            return JsonConvert.DeserializeObject<DataBase>
                (File.ReadAllText(@"..\..\DB\DataBase.json"));
        }

        public static void WriteDataToFile(DataBase dB)
        {
            File.WriteAllText(@"..\..\DB\DataBase.json", JsonConvert.SerializeObject(dB));
        }

        public static void ShowDB(DataBase db)
        {
            foreach (var item in db.Cars)
            {
                ConsoleLog(2, $"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nColor: {item.Color} \nModel: {item.Model} \nId: {item.Id}\ncar\n*");
            }
            foreach (var item in db.Planes)
            {
                ConsoleLog(3, $"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nAvia company: {item.AviaComp} \nId: {item.Id}\nplane\n*");
            }
        }
        #endregion
        // Programm execute
        #region
        public static void ProgExecute()
        {
            while (true)
            {
                ConsoleLog(1, "item do you want to change: car or plane?");
                tryAgain:
                var item = Console.ReadLine().ToLower();
                if (item != "car" && item != "plane")
                {
                    ConsoleLog(1, "Please, enter \"car\" or \"plane\"");
                    goto tryAgain;
                }

                ConsoleLog(1, $"1-remove {item} (by ID)\n2-add default {item} \n3-add {item} with parameters");

                var choice = Choose();

                var id = CreateRandomId(item);

                CommandHandler(id, item, choice);

                ShowDB(DB);
                ConsoleLog(1, "If you want to exit, type 0, or anything else to continue: ");

                if (Int32.TryParse(Console.ReadLine(), out int input) && input == 0)
                {
                    break;
                }
            }
        }

        public static void CommandHandler(int id, string item, int choice)
        {
            switch (choice)
            {
                case 1:
                    RemoveItemById(item);
                    break;
                case 2:
                    AddNewDefaultItem(id, item);
                    break;
                case 3:
                    AddNewCustomItem(id, item);
                    break;
            }
            WriteDataToFile(DB);
        }

        public static void AddNewCustomItem(int id, string item)
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
                if (item == "car")
                {
                    Console.Write("Model: ");
                    var cpar1 = Console.ReadLine();
                    Console.Write("Color: ");
                    var cpar2 = Console.ReadLine();
                    DB.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, id));
                }
                else if (item == "plane")
                {
                    var ppar1 = Console.ReadLine();
                    Console.Write("Amount of turbines: ");
                    var ppar2 = Convert.ToInt32(Console.ReadLine());
                    DB.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, id));
                }
            }
            catch (Exception exception)
            {
                ConsoleLog(1, "Incorrect format of the last variable, try again");
                goto retry;
            }
        }

        public static void AddNewDefaultItem(int id, string item)
        {
            if (item == "car")
            {
                DB.Cars.Add(new Car(id));
            }
            else if (item == "plane")
                DB.Planes.Add(new Plane(id));
        }

        public static void RemoveItemById(string item)
        {
            retry:
            try
            {
                if ((item == "car" && DB.Cars.Count != 0) || (item == "plane" && DB.Planes.Count != 0))
                {
                    TestOnInput(item);
                }
                else
                {
                    ConsoleLog(1, $"Sorry, there is no {item} in DataBase.");
                }
            }
            catch (Exception exception)
            {
                ConsoleLog(1, $"Wrong {item}`s id! Please, try again.");
                goto retry;
            }
        }
        #endregion
        // Misc
        #region
        public static int CreateRandomId(string item)
        {
            var rand = new Random();
            var id = rand.Next(0, 1000);
            if (item == "car")
            {
                while (DB.Cars.Exists(x => x.Id == id))
                {
                    id = rand.Next(0, 1000);
                }
            }
            else if (item == "plane")
            {
                while (DB.Planes.Exists(x => x.Id == id))
                {
                    id = rand.Next(0, 1000);
                }
            }
            return id;
        }

        public static int Choose()
        {
            theChoice:

            var result = Int32.TryParse(Console.ReadLine(), out int choice);
            if (!result || choice != 1 && choice != 2 && choice != 3)
            {
                ConsoleLog(1, "Choose 1, 2 or 3");
                goto theChoice;
            }
            return choice;
        }

        public static void TestOnInput(string item)
        {
            ConsoleLog(1, $"Insert {item}`s id, or type \"exit\" to cancel");
            var input = Console.ReadLine();
            if (input != "exit")
            {
                var id = Convert.ToInt32(input);
                if (item == "car")
                {
                    var IndexOfTheItem = DB.Cars.FindIndex(x => x.Id == id);
                    DB.Cars.RemoveAt(IndexOfTheItem);
                }
                else if (item == "plane")
                {
                    var IndexOfTheItem = DB.Planes.FindIndex(x => x.Id == id);
                    DB.Planes.RemoveAt(IndexOfTheItem);
                }
            }
        }
        #endregion

    }
}

// TODO: основные правки в коде
// TODO: переделай метод RemoveItemById. тут много повторяющегося кода - исправь это.
// TODO: из всех цветных выводов в консоль сделай один метод, который принимает 2 параметра : ошибку и цвет. ну и соответственно в коде это исправь. ну и тогда перенеси его к прочим методам.
// TODO: сделай НОРМАЛЬНЫЕ названия переменных и методов, чтобы человек, посмотрев на название, МОГ ПОНЯТЬ, за что оно отвечает.
