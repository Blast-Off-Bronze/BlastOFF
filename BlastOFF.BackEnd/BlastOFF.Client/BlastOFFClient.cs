namespace BlastOFF.Client
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using BlastOFF.Services.Constants;

    using Services.Services;

    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;

    using Data;

    public class BlastOFFClient
    {
        public static void Main()
        {
            //string filePath = "../../../miscellaneous files/test.mp3";

            //var service = GoogleDriveService.Get();

            //File body = new File();
            //body.Parents = new List<ParentReference> { new ParentReference() { Id = MusicConstants.GoogleDriveBlastOFFMusicFolderId } };

            //// File's content.
            //byte[] byteArray = System.IO.File.ReadAllBytes(filePath);

            //System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);


            //try
            //{
            //    FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, "audio/mpeg");
            //    request.Upload();
            //    Console.WriteLine(request.ResponseBody);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("An error occurred: " + e.Message);
            //}
        }
    }
}
