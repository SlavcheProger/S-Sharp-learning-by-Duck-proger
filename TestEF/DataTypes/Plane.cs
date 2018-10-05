namespace TestEF
{
    public class Plane : Transport
    {
        public string AviaComp { get; set; }
        public int AmountOfTurb { get; set; }

        public Plane() : this("def. aviaComp", 0, 0, 0.0, 0, 0) { }
        public Plane(string aviaComp, int amountOfTurb, int speed, double fuelConsum, int costOfMaintain, int id)
            : base(speed, fuelConsum, costOfMaintain, id)
        {
            AviaComp = aviaComp;
            AmountOfTurb = amountOfTurb;
        }
    }
}

