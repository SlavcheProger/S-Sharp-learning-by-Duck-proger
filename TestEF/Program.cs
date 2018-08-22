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
                Console.Write("What do you want to change? ");

                var what = Console.ReadLine();

                Console.Write("To remove last item write 0, to add default item write 1 : ");

                var choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 0:
                        if (what == "cars")
                        {
                            DB.Cars.RemoveAt(DB.Cars.Count - 1);
                        }
                        else if (what == "planes")
                        {
                            DB.Planes.RemoveAt(DB.Planes.Count - 1);
                        }

                        break;
                    case 1:
                        if (what == "cars")
                        {
                            DB.Cars.Add(new Car());
                        }
                        else if (what == "planes")
                            DB.Planes.Add(new Plane());
                        break;
                    default:
                        Console.WriteLine("Choose 0 or 1");
                        break;
                }
                WriteDataToFile(DB);
                Console.Write("If you want to exit, enter 0, or any key to continue: ");
                if (Convert.ToInt32(Console.ReadLine()) == 0) { break; }
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
                (File.ReadAllText(@"C:\Users\Миша\Desktop\C#\App\TestEF\DB\DataBase.json"));
        }

        public static void WriteDataToFile(DataBase dB)
        {
                File.WriteAllText(@"C:\Users\Миша\Desktop\C#\App\TestEF\DB\DataBase.json", JsonConvert.SerializeObject(dB));
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
