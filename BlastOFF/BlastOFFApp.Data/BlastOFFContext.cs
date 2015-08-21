namespace BlastOFFApp.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class BlastOFFContext : DbContext
    {
        public BlastOFFContext()
            : base("name=BlastOFFContext")
        {

        }

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }
}