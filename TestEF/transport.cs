namespace TestEF
{
    public class Transport
    {
        public int Speed { get; set; }
        public double FuelConsum { get; set; }
        public int CostOfMaintain { get; set; }

        public Transport(int speed, double fuelConsum, int costOfMaintain) {
            Speed = speed;
            FuelConsum = fuelConsum;
            CostOfMaintain = costOfMaintain;
        }
        public Transport() : this(0,0.0,0) { }
    }
}
