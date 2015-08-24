namespace BlastOFF.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    using BlastOFF.Models.MusicModels;

    internal sealed class Configuration : DbMigrationsConfiguration<BlastOFFContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "BlastOFF.Data.BlastOFFContext";
        }

        protected override void Seed(BlastOFFContext dbo)
        {
            //// Add songs
            if (!dbo.Songs.Any())
            {
                string songsInputFile = "../../../database seed files/songs.txt";

                using (var reader = new StreamReader(songsInputFile))
                {
                    var line = reader.ReadLine();

                    while (line != null)
                    {
                        var songData = line.Split('|');

                        var title = songData[0].Trim();
                        var artist = songData[1].Trim();
                        var filePath = songData[2].Trim();
                        var viewsCount = int.Parse(songData[3].Trim());

                        dbo.Songs.Add(
                            new Song
                            {
                                Title = title,
                                Artist = artist,
                                FilePath = filePath,
                                ViewsCount = viewsCount
                            });

                        line = reader.ReadLine();
                    }
                }

                dbo.SaveChanges();
            }
        }
    }
}