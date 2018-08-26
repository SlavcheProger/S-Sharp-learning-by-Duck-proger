namespace TestEF
{
    public class Transport
    {
        public int Speed { get; set; }
        public double FuelConsum { get; set; }
        public int CostOfMaintain { get; set; }
        public int Id { get; set; }
        public Transport(int speed, double fuelConsum, int costOfMaintain, int id) {
            Speed = speed;
            FuelConsum = fuelConsum;
            CostOfMaintain = costOfMaintain;
            Id = id;
        }
        public Transport(int id) : this(0,0.0,0, id) { }
    }
}
