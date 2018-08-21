using System.Collections.Generic;

namespace TestEF
{
    public class DataBase
    {
        public List<Car> Cars { get; set; }
        public List<Plane> Planes { get; set; }

        public DataBase(List<Car> cars, List<Plane> planes)
        {
            Cars = cars;
            Planes = planes;
        }
        public DataBase() : this(new List<Car>(), new List<Plane>()) { }
    }
}
