define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('musicDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'music/albums';

        function getAllMusicAlbums() {

            var headers = new requestHeaders().get();

            return requester.get(headers, serviceUrl);
        }

        function getAllSongs(albumId) {

            var headers = new requestHeaders().get();
            var url = serviceUrl + '/' + albumId + '/songs';

            return requester.get(headers, serviceUrl);
        }




        // LIKES
        function likeMusicAlbum(albumId) {

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/' + albumId + '/likes';

            return requester.post(headers, url, null);
        }







        function addMusicAlbum(musicAlbum) {

            var headers = new requestHeaders().get();

            return requester.post(headers, serviceUrl, musicAlbum);
        }

        function addSong(musicAlbumId, song) {

            var headers = new requestHeaders().get();
            var url = serviceUrl + '/' + musicAlbumId + '/songs';

            return requester.post(headers, url, song);
        }

        return {
            getAllMusicAlbums: getAllMusicAlbums,
            getAllSongs: getAllSongs,

            // Likes
            likeMusicAlbum:likeMusicAlbum,

            addMusicAlbum: addMusicAlbum,
            addSong: addSong
        }
    });
});