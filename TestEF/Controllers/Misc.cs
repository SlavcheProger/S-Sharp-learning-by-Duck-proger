using System;
using static TestEF.Controllers.AppController;
using System.Linq;

namespace TestEF.Controllers
{
    class Misc
    {
        public static int FindId(Item TransportType)
        {
            int id;
            switch (TransportType)
            {
                default:
                case Item.car:
                    id = db.Cars.Count();
                    break;
                case Item.plane:
                    id = db.Planes.Count();
                    break;
            }

            return id;
        }

        


        public static int ChoiseHandler()
        {
        theChoice:

            var result = Int32.TryParse(Console.ReadLine(), out int choice);
            if (!result || choice != 1 && choice != 2 && choice != 3 && choice != 4)
            {
                Log.ConsoleLog(ConsoleColor.Yellow, "Choose 1, 2, 3 or 4");
                goto theChoice;
            }
            return choice;
        }

        public static void TestOnInput(Item TransportType, TransportContext DB)
        {
            Log.ConsoleLog(ConsoleColor.Yellow, $"Insert {TransportType}`s id, or type \"exit\" to cancel");
            var input = Console.ReadLine();
            if (input != "exit")
            {
                var id = Convert.ToInt32(input);
                switch (TransportType)
                {
                    case Item.car:
                        var CarForDeletion = db.Cars.Find(id);

                        db.Cars.Remove(CarForDeletion);

                        db.SaveChanges();

                        break;
                    case Item.plane:
                        var PlaneForDeletion = db.Planes.Find(id);

                        db.Planes.Remove(PlaneForDeletion);

                        db.SaveChanges();
                        break;
                }
            }
        }
    }
}
