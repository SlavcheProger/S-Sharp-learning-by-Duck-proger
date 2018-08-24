﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace TestEF
{
    public class Program
    {
        public static DataBase DB;
        // теперь метод Main выглядит, как должен выглядеть :)
        static void Main(string[] args)
        {
            try
            {
                DB = LoadDB();
                ProgExecute();
            }
            catch (Exception exception)
            {
                Log(exception);
            }
            finally
            {
                Console.Write("Press any key to quit: ");
                Console.ReadKey();
            }
        }
        // console color output
        #region
        //for errors
        public static void Log(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(exception.ToString());
            Console.ResetColor();
            Console.WriteLine();
        }
        //basic
        public static void CLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(str);
            Console.ResetColor();
            Console.WriteLine();
        }
        //for cars
        public static void CarLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(str);
            Console.ResetColor();
            Console.WriteLine();
        }
        //for planes
        public static void PlaneLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
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
                Log(exception);
                if (!File.Exists(@"..\..\DB\DataBase.json"))
                {
                    CLog("Data Base missing, creating a new one");
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
                CarLog($"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nColor: {item.Color} \nModel: {item.Model} \nId: {item.Id}\ncar\n*");
            }
            foreach (var item in db.Planes)
            {
                PlaneLog($"Speed: {item.Speed}\nCost of maintain: {item.CostOfMaintain} \nFuel consumation: {item.FuelConsum} \nAvia company: {item.AviaComp} \nId: {item.Id}\nplane\n*");
            }
        }
        #endregion
        // Programm execute
        #region
        public static void ProgExecute()
        {
            while (true)
            {
                CLog("What do you want to change: car or plane?");
                tryAgain:
                var what = Console.ReadLine().ToLower();
                if (what != "car" && what != "plane")
                {
                    CLog("Please, enter \"car\" or \"plane\"");
                    goto tryAgain;
                }

                CLog($"1-remove {what} (by ID)\n2-add default {what} \n3-add {what} with parameters");

                var choice = Choose();

                var id = RandId(what);

                DBChange(id, what, choice);

                ShowDB(DB);
                CLog("If you want to exit, type 0, or anything else to continue: ");

                if (Int32.TryParse(Console.ReadLine(), out int input) && input == 0)
                {
                    break;
                }
            }
        }

        public static void DBChange(int id, string what, int choice)
        {
            switch (choice)
            {
                case 1:
                    RemoveSmth(what);
                    break;
                case 2:
                    AddSmth(id, what);
                    break;
                case 3:
                    AddCustom(id, what);
                    break;
            }
            WriteDataToFile(DB);
        }

        public static void AddCustom(int id, string what)
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
                if (what == "car")
                {
                    Console.Write("Model: ");
                    var cpar1 = Console.ReadLine();
                    Console.Write("Color: ");
                    var cpar2 = Console.ReadLine();
                    DB.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, id));
                }
                else if (what == "plane")
                {
                    var ppar1 = Console.ReadLine();
                    Console.Write("Amount of turbines: ");
                    var ppar2 = Convert.ToInt32(Console.ReadLine());
                    DB.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, id));
                }
            }
            catch (Exception exception)
            {
                CLog("Incorrect format of the last variable, try again");
                goto retry;
            }
        }

        public static void AddSmth(int id, string what)
        {
            if (what == "car")
            {
                DB.Cars.Add(new Car(id));
            }
            else if (what == "plane")
                DB.Planes.Add(new Plane(id));
        }

        public static void RemoveSmth(string what)
        {
            retry:
            var input = "";
            try
            {
                if (what == "car" && DB.Cars.Count != 0)
                {
                    CLog($"Insert {what}`s id, or type \"exit\" to cancel");
                    input = Console.ReadLine();
                    if (input != "exit")
                    {
                        var id = Convert.ToInt32(input);
                        var that = DB.Cars.FindIndex(x => x.Id == id);
                        DB.Cars.RemoveAt(that);
                    }
                }
                else if (what == "plane" && DB.Planes.Count != 0)
                {
                    CLog($"Insert {what}`s id, or type \"exit\" to cancel");
                    input = Console.ReadLine();
                    if (input != "exit")
                    {
                        var id = Convert.ToInt32(input);
                        var that = DB.Planes.FindIndex(x => x.Id == id);
                        DB.Planes.RemoveAt(that);
                    }
                }
                else
                {
                    CLog($"Sorry, there is no {what} in DataBase.");
                }
            }
            catch (Exception exception)
            {
                CLog($"Wrong {what}`s id! Please, try again.");
                goto retry;
            }
        }
        #endregion
        // Misc
        #region
        public static int RandId(string what)
        {
            var rand = new Random();
            var id = rand.Next(0, 1000);
            if (what == "car")
            {
                while (DB.Cars.Exists(x => x.Id == id))
                {
                    id = rand.Next(0, 1000);
                }
            }
            else if (what == "plane")
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
                CLog("Choose 1, 2 or 3");
                goto theChoice;
            }
            return choice;
        }
        #endregion

    }
}

// TODO: основные правки в коде
// TODO: переделай метод RemoveSmth. тут много повторяющегося кода - исправь это.
// TODO: из всех цветных выводов в консоль сделай один метод, который принимает 2 параметра : ошибку и цвет. ну и соответственно в коде это исправь. ну и тогда перенеси его к прочим методам.
// TODO: сделай НОРМАЛЬНЫЕ названия переменных и методов, чтобы человек, посмотрев на название, МОГ ПОНЯТЬ, за что оно отвечает.
