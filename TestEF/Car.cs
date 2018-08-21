namespace TestEF
{
    public class Car : Transport
    {
        public string Model { get; set; }
        public string Color { get; set; }

        public Car() : this("def. model", "def. colour", 0, 0.0, 0) { }
        public Car(string model, string color, int speed, double fuelConsum, int costOfMaintain)
            : base(speed, fuelConsum, costOfMaintain)
        {
            Model = model;
            Color = color;
        }
    }
}
