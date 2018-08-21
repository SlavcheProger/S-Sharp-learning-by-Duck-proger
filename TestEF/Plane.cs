namespace TestEF
{
    public class Plane : Transport
    {
        public string AviaComp { get; set; }
        public int AmountOfTurb { get; set; }

        public Plane() : this("def. aviaComp", 0, 0, 0.0, 0) { }
        public Plane(string aviaComp, int amountOfTurb, int speed, double fuelConsum, int costOfMaintain)
            : base(speed, fuelConsum, costOfMaintain)
        {
            AviaComp = aviaComp;
            AmountOfTurb = amountOfTurb;
        }
    }
}

