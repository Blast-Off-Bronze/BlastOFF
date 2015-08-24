namespace BlastOFF.Client
{
    using System;
    using System.Linq;

    using Data;

    public class BlastOFFClient
    {
        public static void Main()
        {
            var dbo = new BlastOFFContext();

            var songsCount = dbo.Songs.Count();
            Console.WriteLine("Songs count: {0}", songsCount);
        }
    }
}