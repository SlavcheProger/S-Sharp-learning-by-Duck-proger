using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace TestEF
{
    class TransportContext : DbContext
    {
        public TransportContext()
            : base("DbConnection")
        { }

        public DbSet<DataBase> Transport { get; set; }
    }
}
