using Newtonsoft.Json;
using System;
using System.IO;

namespace TestEF.Controllers
{
    class DataBaseController
    {
        public static TransportContext LoadDB() {
            return new TransportContext();
        }
        public static void WriteDataToFile(TransportContext  dB)
        {
            File.WriteAllText(@"..\..\DB\DataBase.json", JsonConvert.SerializeObject(dB));
        }

        public static void ShowDB(TransportContext  db)
        {
            foreach (var itemType in db.Cars)
            {
                Log.ConsoleLog(ConsoleColor.Cyan, $"Speed: {itemType.Speed}\nCost of maintain: {itemType.CostOfMaintain} \nFuel consumation: {itemType.FuelConsum} \nColor: {itemType.Color} \nModel: {itemType.Model} \nId: {itemType.Id}\ncar\n*");
            }
            foreach (var itemType in db.Planes)
            {
                Log.ConsoleLog(ConsoleColor.Red, $"Speed: {itemType.Speed}\nCost of maintain: {itemType.CostOfMaintain} \nFuel consumation: {itemType.FuelConsum} \nAvia company: {itemType.AviaComp} \nId: {itemType.Id}\nplane\n*");
            }
        }
    }
}
