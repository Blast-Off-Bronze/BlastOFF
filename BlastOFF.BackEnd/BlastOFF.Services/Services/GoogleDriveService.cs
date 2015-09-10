using System.IO;

namespace BlastOFF.Services.Services
{
    using System.Security.Cryptography.X509Certificates;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v2;
    using Google.Apis.Services;

    public static class GoogleDriveService
    {
        private const string GoogleDriveServiceAccountEmail = "549251813735-6efg2gfp38a6q4moeknilsk2e9n9ajos@developer.gserviceaccount.com";
        private const string GoogleDriveKeyFilePath = @"D:\GitHub\BlastOFF\BlastOFF.BackEnd\BlastOFF.Services\Credentials\BlastOFF.p12";

        public static DriveService Get()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

            var certificate = new X509Certificate2(GoogleDriveKeyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credentials = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(GoogleDriveServiceAccountEmail) { Scopes = scopes }
                        .FromCertificate(certificate));

            return new DriveService(new BaseClientService.Initializer { HttpClientInitializer = credentials });
        }
    }
}