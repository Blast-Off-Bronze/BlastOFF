namespace BlastOFF.Services.Services
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v2;
    using Google.Apis.Services;

    public static class GoogleDriveService
    {
        private const string GoogleDriveServiceAccountEmail ="549251813735-6efg2gfp38a6q4moeknilsk2e9n9ajos@developer.gserviceaccount.com";

        private const string GoogleDrivePrivateKey =
            "-----BEGIN PRIVATE KEY-----\nMIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQDEhrt9dNh+6iPe\nABdQfR+YiYae0+HpHOkIP8jwOsO/eP/MI6sbedWZ83PUjYrLa6B2VHeydUqwDjYR\nASA2MZm9N27k1lCuzxUpsGud9uXzr7/Ugxkrv3gePsaJBEvY7wW45kC/f+w61s1F\nZdF7btMom6hUKqL578qHl9gAWzvmjGhBfhp0TTX9b6vjzvSjWhW9IDg3XzSzggOg\nioNTQ5r+VLz8xQDHOnc04FT9yVRt49nweQQLEaL+i7r7q0qv973WhH1V6MwT0iMK\n+bvUp1YDAs4+5PSN6HnVsmK04IaUwSSESAkqYADrrda9E9S7DgwGsYDzCuCvuOQX\nXLDAhx81AgMBAAECggEBAKWFzRhHd6i9T+RFM13JZPk2q7nOP7H1dZhLENS0yfXU\n1a18RTtpDC0UTXSymjnmtkmzrvURsQxDi+oahqTeddxWegInN+Fj7TEltB5hux/Z\n4Ln+iQ69v0/KE7GHetKFzs4CsHoaJj8Qd+eBcESD/TlwkQACwS29d1lo3LbAIN11\n3hkJJgUalZpcrbKNEnoBS003BJ/xTtCPMfsYvKMrEjNicwEDqIYYyrJJd6I+B/cv\n//BPmZxhHijIWxaPib9ijUVMSA7kRy5hWIuAD5tQfvhIhHbCe0iXVmL17QIfT8BW\nnVkjOCe10w0A71RI/8AeM4gBn2/R0EAyi8ki4rNa+gECgYEA75a0FSMJt08JoUeC\n7BagY6HZjA55fGhZfxe9Er/8k/VWpn7nCkeqDAvJKjjENhA1AZ0Ndj4AqoQbN7TI\nib27l7L5a4Gb/ET8xjcO4I5EcTnC+4D1jV/aNxuIk44DcQpiJhLwTwbPTY9GF3fY\nFBtxT6GSQoC8dOE9Ulh5KX+NuHUCgYEA0fzpD8d9KJkvC6mKBKO/rCiREHjTi5g4\ngaq2DDQSsmkrUtnjX20XEfqb9VVqlaLx3bHur1Bxo7LoVj03svkF/DHgU1jug8Dh\nXs8Sm2fHQWQeZj9YZ1xGDKhjvs7MdBIehR4ldML8nE6YBC7nwGVL0PxwwIgPstI+\ns8eYdfs488ECgYEAmLSaRRqW98Ilpij5NlbZlYc0LIORRQ0RdeMAfLHnX7qwpuAd\njDzPEmx0pDeYP8kmr8eVK/cq34PiASh15O0MYw0M0kvCyxCBeCjhj5i94NjrAPha\nCei9IgCnlTyfzu3KTvAlQIlbmdim2RBmpbI0D4gwu7hn5asmNZT0bNHC130CgYEA\nrPlZfm8EkUnF8CRjFblRbV4pSNlO4SGuAln+BgVrFV3+mwvy+StioqO+jpQi7UpB\nVC57AB0SzxbTvh5FYH8zR/BJ6j5Kk2tx+mg22p7dCuePBwjNKK9g4JJhP88XqdE3\nJM6Vah4oehVfsap1qw2GK4uW0XIDBHmdvvf+hclAu8ECgYAd47WKU7fMVNi4rdln\nb+DNFu6Dtks+KMv92wCHFxqOvnQbiXQJY7VcdSI62/RQAJokM8DCUcjTMzGkqkxw\nakp0cUWBdZqjJnhd2Ie/cuD+Pbaj0sUzo1c5yBeheQc8hE+A1m6lOzrcC8qjxLQI\n8tuQgnSmmJe2DIbmJH/DkILSjw\u003d\u003d\n-----END PRIVATE KEY-----\n";

        public static DriveService Get()
        {
            string[] scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

            var credentials =
                new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(GoogleDriveServiceAccountEmail) { Scopes = scopes }.FromPrivateKey(GoogleDrivePrivateKey));

            return new DriveService(new BaseClientService.Initializer { HttpClientInitializer = credentials });
        }
    }
}