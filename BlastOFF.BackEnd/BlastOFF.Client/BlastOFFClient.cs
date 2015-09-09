namespace BlastOFF.Client
{
    using System;
    using System.Linq;

    using BlastOFF.Data;

    public class BlastOFFClient
    {
        public static void Main()
        {
            var dbo = new BlastOFFContext();

            var songsCount = dbo.Songs.Count();

            Console.WriteLine("Total songs: " + songsCount);

            var albums = dbo.MusicAlbums.ToList();

            foreach (var musicAlbum in albums)
            {
                Console.WriteLine(musicAlbum.Followers.Count);
            }
        }
    }
}