using Newtonsoft.Json;
using System;
using System.IO;

namespace TestEF
{
    public class Program
    {
        static void Main(string[] args)
        {
            DataBase DB;
            try
            {
                DB = ReadDB();
                ShowDB(DB);
            }
            catch (Exception exception)
            {
                Log(exception);
                CLog("Data Base missing, creating a new one");
                FileStream DataBase = File.Create(@"..\..\DB\DataBase.json");
                DB = new DataBase();
            }


            while (true)
            {
                CLog("What do you want to change: car or plane?");
                int dumbcount = 0;
                tryAgain:
                var what = Console.ReadLine().ToLower();
                if(what !="car"){
                        if(what != "plane"){
                            if(dumbcount>2){CLog("R U stupid? ");}
                            CLog("Please, enter \"car\" or \"plane\"");
                            dumbcount += 1;
                            goto tryAgain;
                        }
                }
                CLog($"0-remove {what} (by ID)\n1-add default {what} \n2-add {what} with parameters");
                theChoice: //флаг для goto, при вводе некорректного значения выбора действия
                var choice = Convert.ToInt32(Console.ReadLine()); 

                var rand = new Random();
                var id = rand.Next(0, 1000);
                if (what == "car")
                {
                    while(DB.Cars.Exists(x => x.Id == id))
                    {
                        id = rand.Next(0, 1000);
                    }                            
                }
                else if (what == "plane")
                {
                    while(DB.Planes.Exists(x => x.Id == id))
                    {
                        id = rand.Next(0, 1000);
                    }   
                }
                switch (choice)
                {
                    case 0:
                        try
                        {
                            if (what == "car" && DB.Cars.Count != 0) 
                            {
                                CLog($"Insert {what}`s id");

                                id = Convert.ToInt32(Console.ReadLine());
                                var that = DB.Cars.FindIndex(x => x.Id == id);
                                DB.Cars.RemoveAt(that);
                            }
                            else if (what == "plane" && DB.Planes.Count != 0)
                            {
                                CLog($"Insert {what}`s id");

                                id = Convert.ToInt32(Console.ReadLine());

                                var that = DB.Planes.FindIndex(x => x.Id == id);
                                DB.Planes.RemoveAt(that);
                            }
                            else
                            {
                                CLog($"Sorry, there is no {what} in DataBase.");
                            }
                        }
                        catch (Exception exception)
                        {
                            Log(exception);
                            CLog($"Wrong {what}`s id! Please, try again.");
                            goto case 0;
                        }
                        break;
                    case 1:
                        if (what == "car")
                        {
                            DB.Cars.Add(new Car(id));
                        }
                        else if (what == "plane")
                            DB.Planes.Add(new Plane(id));
                        break;
                    case 2:
                        if (what == "car")
                        {
                            try
                            {
                                Console.Write("Speed: ");
                                var par1 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Fuel consumation: ");
                                var par2 = Convert.ToDouble(Console.ReadLine());
                                Console.Write("Cost of maintaining: ");
                                var par3 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Model: ");
                                var cpar1 = Console.ReadLine();
                                Console.Write("Color: ");
                                var cpar2 = Console.ReadLine();
                                DB.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, id));
                            }
                            catch (Exception exception)
                            {
                            CLog("Incorrect format of the last variable, try again");                            
                            goto case 2;
                            }
                        }
                        else if (what == "plane")
                        {
                            try
                            {
                                Console.Write("Speed: ");
                                var par1 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Fuel consumation: ");
                                var par2 = Convert.ToDouble(Console.ReadLine());
                                Console.Write("Cost of maintaining: ");
                                var par3 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Avia company: ");
                                var ppar1 = Console.ReadLine();
                                Console.Write("Amount of turbines: ");
                                var ppar2 = Convert.ToInt32(Console.ReadLine());
                                DB.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, id));                               
                            }
                            catch (Exception exception)
                            {
                            CLog("Incorrect format of the last variable, try again");
                            goto case 2;
                            }
                        }
                        break;
                    default:
                        CLog("Choose 0, 1 or 2");
                        goto theChoice;
                }                  
                        WriteDataToFile(DB);
                        ShowDB(DB);
                CLog("If you want to exit, enter 0, or press enter key to continue: ");

                try
                {
                    if (Convert.ToInt32(Console.ReadLine()) == 0)
                    {
                        break;
                    }
                }
                catch (Exception exception)
                {
                    //сделал из ошибки фичу
                }
            }
            /////////////////////////
        }

        public static void Log(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(exception.ToString());
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void CLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(str);
            Console.ResetColor();
            Console.WriteLine();
        }
        public static void CarLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(str);
            Console.ResetColor();
            Console.WriteLine();
        }
        public static void PlaneLog(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ResetColor();
            Console.WriteLine();
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
    }
}
