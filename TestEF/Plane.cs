namespace TestEF
{
    public class Plane : Transport
    {
        public string AviaComp { get; set; }
        public int AmountOfTurb { get; set; }

        public Plane() : this(0) { }
        public Plane(int id) : this("def. aviaComp", 0, 0, 0.0, 0, id) { }
        public Plane(string aviaComp, int amountOfTurb, int speed, double fuelConsum, int costOfMaintain, int id)
            : base(speed, fuelConsum, costOfMaintain, id)
        {
            AviaComp = aviaComp;
            AmountOfTurb = amountOfTurb;
        }
    }
}

