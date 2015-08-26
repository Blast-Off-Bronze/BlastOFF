namespace BlastOFF.Client
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;
    using Google.Apis.Services;

    public class BlastOFFClient
    {
        public static void Main()
        {
            //// var dbo = new BlastOFFContext();

            //// var songsCount = dbo.Songs.Count();
            //// Console.WriteLine("Songs count: {0}", songsCount);

            string[] scopes = new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

            var keyFilePath = "../../../google drive api key file/BlastOFF.p12";
            var serviceAccountEmail = "549251813735-6efg2gfp38a6q4moeknilsk2e9n9ajos@developer.gserviceaccount.com";

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credentials =
                new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail) { Scopes = scopes }.FromCertificate(
                        certificate));

            var service = new DriveService(new BaseClientService.Initializer { HttpClientInitializer = credentials });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.MaxResults = 1000;

            // List files.
            IList<File> files = listRequest.Execute().Items;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Title, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }

            Console.Read();

            //Google.Apis.Drive.v2.Data.File body = new Google.Apis.Drive.v2.Data.File();
            //body.Title = "My document";
            //body.Description = "A test document";
            //body.MimeType = "text/plain";
            //body.Parents = new List<ParentReference>() { new ParentReference() { Id = "0ByDHCWWSmvcLfmVza0FVV3U4LTF2SnpWdDBDMUdoUXVVY21WbE5kMF9QQlB1SjdXamp0SWM" } };

            //byte[] byteArray = System.IO.File.ReadAllBytes("../../../google drive api key file/document.txt");
            //System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            //FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, "text/plain");
            //request.Upload();

            //Google.Apis.Drive.v2.Data.File file = request.ResponseBody;
            //Console.WriteLine("File id: " + file.Id);
            //Console.WriteLine("Press Enter to end this process.");
            //Console.ReadLine();
        }
    }
}