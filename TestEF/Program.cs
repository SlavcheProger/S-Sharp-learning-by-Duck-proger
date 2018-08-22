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
                DB = new DataBase();
            }

            while (true)
            {
                Console.Write("To remove last item write 0, to add default item write 1 : ");

                var choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        var what = Console.ReadLine();

                        if (what == "cars")
                            DB.Cars.RemoveAt(DB.Cars.Count - 1);
                        else if (what == "planes")
                            DB.Planes.RemoveAt(DB.Planes.Count - 1);
                        break;
                    case 1:
                        //todo: adding to db and saving to the .json file
                        break;
                    default:
                        Console.Write("Choose 0 or 1");
                        break;
                }
                WriteDataToFile(DB);
            }

            /////////////////////////
            Console.WriteLine("Succeed!");
            Console.ReadKey();
        }

        public static void Log(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(exception.ToString());
            Console.ResetColor();
            Console.WriteLine();
        }

        public static DataBase ReadDB()
        {
            return JsonConvert.DeserializeObject<DataBase>
                (File.ReadAllText(@"C:\Users\Миша\Desktop\C#\App\TestEF\DB\DataBase.json")); //todo: add filepath
        }
        public static void WriteDataToFile(DataBase dB)
        {
            //todo: add saving database to the .json file
        }
        public static void ShowDB(DataBase db)
        {
            foreach (var item in db.Cars)
            {
                Console.WriteLine($"{item.Speed},{item.CostOfMaintain}, {item.FuelConsum}, {item.Color}, {item.Model}");
            }
            foreach (var item in db.Planes)
            {
                Console.WriteLine($"{item.Speed},{item.CostOfMaintain}, {item.FuelConsum}, {item.AviaComp}, {item.AmountOfTurb}");
            }
        }
    }
}
