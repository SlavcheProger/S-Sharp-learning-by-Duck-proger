using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace TestEF
{
    public class TransportContext : DbContext
    {
        public TransportContext() : base("DbConnection") { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Plane> Planes { get; set; }
    }
}
