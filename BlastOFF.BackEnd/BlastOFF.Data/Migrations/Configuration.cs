namespace BlastOFF.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    using BlastOFF.Models.MusicModels;
    using BlastOFF.Models.UserModel;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<BlastOFFContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            this.ContextKey = "BlastOFF.Data.BlastOFFContext";
        }

        //protected override void Seed(BlastOFFContext dbo)
        //{
        //    // Add songs
        //    if (!dbo.Songs.Any())
        //    {
        //        string songsInputFile = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "database seed files/songs.txt");

        //        using (var reader = new StreamReader(songsInputFile))
        //        {
        //            var line = reader.ReadLine();

        //            while (line != null)
        //            {
        //                var songData = line.Split('|');

        //                var id = songData[0].Trim();
        //                var title = songData[1].Trim();
        //                var artist = songData[2].Trim();
        //                var filePath = "http://docs.google.com/uc?export=open&id=" + id;

        //                var song = new Song
        //                {
        //                    Title = title,
        //                    Artist = artist,
        //                    FilePath = filePath,
        //                    DateAdded = DateTime.Now,
        //                };

        //                dbo.Songs.Add(song);

        //                line = reader.ReadLine();
        //            }
        //        }

        //        dbo.SaveChanges();
        //    }
        //}
    }
}