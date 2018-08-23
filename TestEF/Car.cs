namespace TestEF
{
    public class Car : Transport
    {
        public string Model { get; set; }
        public string Color { get; set; }

        public Car() : this(0) { }
        public Car(int id) : this("def. model", "def. colour", 0, 0.0, 0, id) { }
        public Car(string model, string color, int speed, double fuelConsum, int costOfMaintain, int id)
            : base(speed, fuelConsum, costOfMaintain, id)
        {
            Model = model;
            Color = color;
        }
    }
}
