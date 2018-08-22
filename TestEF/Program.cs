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
                Console.WriteLine("What do you want to change? ");

                var what = Console.ReadLine();

                Console.WriteLine("0-remove last item \n1-add default item \n2-add item with parameters");

                var choice = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Insert item`s id");

                var id = Convert.ToInt32(Console.ReadLine()); //пользователь ввел некое РАНДОМНОЕ число

                if (((choice == 0) | (choice == 1) | (choice == 2))
                  && (id != DB.Cars.Count)) //не понимаю смысла этой проверки - объясни пж
                                            //сюда же - ты проверяешь только машины - забыл про самолеты
                {
                    if (what == "cars")
                    {
                        DB.Cars.RemoveAt(id);
                    }
                    else if (what == "planes")
                    {
                        DB.Planes.RemoveAt(id);
                    }
                }

                switch (choice)
                {
                    case 0:
                        //где код???
                        break;
                    case 1:
                        if (what == "cars")
                        {
                            DB.Cars.Insert(id, new Car(id));
                        }
                        else if (what == "planes")
                            DB.Planes.Insert(id, new Plane(id));
                        break;
                    case 2:
                        Console.Write("Speed: ");
                        var par1 = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Fuel consumation: ");
                        var par2 = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Cost of maintaining: ");
                        var par3 = Convert.ToInt32(Console.ReadLine());
                        if (what == "cars")
                        {
                            Console.Write("Model: ");
                            var Cpar1 = Console.ReadLine();  //переменные пишем с маленькой буквы - исправить везде
                            Console.Write("Color: ");
                            var Cpar2 = Console.ReadLine();
                            DB.Cars.Add(new Car(Cpar1, Cpar2, par1, par2, par3, id));
                        }
                        else if (what == "planes")
                        {
                            Console.Write("Avia company: ");
                            var Ppar1 = Console.ReadLine();
                            Console.Write("Amount of turbines: ");
                            var Ppar2 = Convert.ToInt32(Console.ReadLine());
                            DB.Planes.Add(new Plane(Ppar1, Ppar2, par1, par2, par3, id));
                        }
                        break;
                    default:
                        Console.WriteLine("Choose 0, 1 or 2");
                        break;
                }
                WriteDataToFile(DB);
                ShowDB(DB);
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
