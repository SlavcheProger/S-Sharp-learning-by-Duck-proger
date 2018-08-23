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
                Console.WriteLine("What do you want to change: car or plane?");

                var what = Console.ReadLine().ToLower(); // нужно привести в какой то единый регистр, например нижний
                //и я говорил, что надо обрабатывать все то, что вводит пользователь - добавь это
                Console.WriteLine($"0-remove {what} (by ID)\n1-add default {what} \n2-add {what} with parameters");
                //так прикольнее :)

                var choice = Convert.ToInt32(Console.ReadLine());  //крч везде проверку на корректность добавь, иначе будет все крашиться

                var rand = new Random();
                var id = rand.Next(0, 1000);

                switch (choice)
                {
                    case 0:
                        try
                        {
                            if (what == "car" && DB.Cars.Count != 0) // тут была трабла, что ты пытаешься удалить элемент из пустой бд - это тоже надо обрабатывать
                            {
                                Console.WriteLine($"Insert {what}`s id");

                                id = Convert.ToInt32(Console.ReadLine());

                                DB.Cars.RemoveAt(id);
                            }
                            else if (what == "plane" && DB.Planes.Count != 0)
                            {
                                Console.WriteLine($"Insert {what}`s id");

                                id = Convert.ToInt32(Console.ReadLine());

                                DB.Planes.RemoveAt(id);
                            }
                            else
                            {
                                Console.WriteLine($"Sorry, where is no {what} in DataBase.");
                            }
                        }
                        catch (Exception exception)
                        {
                            Log(exception);
                            Console.WriteLine($"Wrong {what}`s id! Please, try again.");
                            // и тогда у тебя не будет проблемы, что пользователь будет вечно пытаться ввести ID для удаления чего-либо из пустой БД (у тебя тут был вечный цикл пока приложение не закрашится)
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
                        Console.Write("Speed: ");
                        var par1 = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Fuel consumation: ");
                        var par2 = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Cost of maintaining: ");
                        var par3 = Convert.ToInt32(Console.ReadLine());
                        if (what == "cars")
                        {
                            Console.Write("Model: ");
                            var cpar1 = Console.ReadLine();  //переменные пишем с маленькой буквы - исправить везде
                            Console.Write("Color: ");
                            var cpar2 = Console.ReadLine();
                            DB.Cars.Add(new Car(cpar1, cpar2, par1, par2, par3, id));
                        }
                        else if (what == "plane")
                        {
                            Console.Write("Avia company: ");
                            var ppar1 = Console.ReadLine();
                            Console.Write("Amount of turbines: ");
                            var ppar2 = Convert.ToInt32(Console.ReadLine());
                            DB.Planes.Add(new Plane(ppar1, ppar2, par1, par2, par3, id));
                        }
                        break;
                    default:
                        Console.WriteLine("Choose 0, 1 or 2");
                        break;
                }
                WriteDataToFile(DB);
                ShowDB(DB);

                Console.Write("If you want to exit, enter 0, or any key to continue: ");

                try
                {
                    if (Convert.ToInt32(Console.ReadLine()) == 0)
                    {
                        break;
                    }
                }
                catch (Exception exception)
                {
                    Log(exception);
                }
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
                Console.WriteLine($"{item.Speed},{item.CostOfMaintain}, {item.FuelConsum}, {item.Color}, {item.Model}, {item.Id}");
            }
            foreach (var item in db.Planes)
            {
                Console.WriteLine($"{item.Speed},{item.CostOfMaintain}, {item.FuelConsum}, {item.AviaComp}, {item.Id}");
            }
        }
    }
}
