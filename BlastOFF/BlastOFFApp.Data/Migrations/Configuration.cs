namespace BlastOFFApp.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BlastOFFApp.Data.BlastOFFContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BlastOFFApp.Data.BlastOFFContext context)
        {
            
        }
    }
}
