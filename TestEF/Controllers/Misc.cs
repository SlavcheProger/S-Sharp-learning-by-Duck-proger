using System;
using static TestEF.Controllers.AppController;
using System.Linq;

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
    }
}
