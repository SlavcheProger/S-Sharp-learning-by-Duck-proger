using System;
using TestEF.DataTypes;

namespace TestEF.Controllers
{
    class Misc
    {
        public static int ChoiceOfAction()
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

        public static int GetId(TransportTypes.Item transportType, TransportContext db)
        {
        retry:
            try
            {
                Console.Write("Enter item's Id: ");
                var id = Convert.ToInt32(Console.ReadLine());
                if ((db.Planes.Find(id) == null && transportType == TransportTypes.Item.plane) || (db.Cars.Find(id) == null && transportType == TransportTypes.Item.car))
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, $"Sorry, there is no {transportType} in DataBase.");
                    goto retry;
                }
                return id;
            }
            catch (Exception)
            {
                Log.ConsoleLog(ConsoleColor.Yellow, "Incorrect Id, try again");
                goto retry;
            }
        }
    }
}
