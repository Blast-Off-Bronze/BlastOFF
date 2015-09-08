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

        protected override void Seed(BlastOFFContext dbo)
        {
            //// Add songs
            // if (!dbo.Songs.Any())
            // {
            // string songsInputFile = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "database seed files/songs.txt");

            // using (var reader = new StreamReader(songsInputFile))
            // {
            // var line = reader.ReadLine();

            // while (line != null)
            // {
            // var songData = line.Split('|');

            // var id = songData[0].Trim();
            // var title = songData[1].Trim();
            // var artist = songData[2].Trim();
            // var filePath = "http://docs.google.com/uc?export=open&id=" + id;

            // var song = new Song
            // {
            // Id = id,
            // Title = title,
            // Artist = artist,
            // FilePath = filePath,
            // ViewsCount = 0,
            // DateAdded = DateTime.Now,
            // MusicAlbumId = dbo.MusicAlbums.OrderBy(a => Guid.NewGuid()).First().Id
            // };

            // song.UploaderId = dbo.MusicAlbums.First(a => a.Id == song.MusicAlbumId).AuthorId;

            // dbo.Songs.Add(song);

            // line = reader.ReadLine();
            // }
            // }

            // dbo.SaveChanges();
            // }
        }
    }
}