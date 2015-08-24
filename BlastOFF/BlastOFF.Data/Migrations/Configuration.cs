namespace BlastOFF.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BlastOFFContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "BlastOFF.Data.BlastOFFContext";
        }

        protected override void Seed(BlastOFFContext context)
        {
        }
    }
}