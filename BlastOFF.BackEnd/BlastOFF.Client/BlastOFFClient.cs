namespace BlastOFF.Client
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Services.Services;

    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;
    using Data;

    public class BlastOFFClient
    {
        public static void Main()
        {
            //var dbo = new BlastOFFContext();

            //var songsCount = dbo.Songs.Count();
            //Console.WriteLine("Songs count: {0}", songsCount);

            //var service = GoogleDriveService.Get();

            //FilesResource.ListRequest listRequest = service.Files.List();

            //// List files.
            //IList<File> files = listRequest.Execute().Items;

            //if (files != null && files.Count > 0)
            //{
            //    foreach (var file in files)
            //    {
            //        Console.WriteLine("{0} (https://drive.google.com/open?id={1})", file.Title, file.Id);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No files found.");
            //}
        }
    }
}