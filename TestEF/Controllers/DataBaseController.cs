using Newtonsoft.Json;
using System;
using System.IO;

namespace TestEF.Controllers
{
    class DataBaseController
    {
        public static DataBase LoadDB(DataBase DB)
        {
            try
            {
                DB = ReadDB();
                ShowDB(DB);
            }
            catch (Exception exception)
            {
                Log.ConsoleLog(ConsoleColor.Green, exception.ToString());
                if (!File.Exists(@"..\..\DB\DataBase.json"))
                {
                    Log.ConsoleLog(ConsoleColor.Yellow, "Data Base missing, creating a new one");
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
