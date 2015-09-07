using System;
using System.Linq;
using BlastOFF.Data;

namespace BlastOFF.Client
{
    public class BlastOFFClient
    {
        public static void Main()
        {
            var dbo = new BlastOFFContext();

            var songsCount = dbo.Songs.Count();

            Console.WriteLine(songsCount);
        }
    }
}