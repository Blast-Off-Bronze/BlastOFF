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

        function unlikeMusicAlbum(albumId) {

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/' + albumId + '/likes';

            return requester.remove(headers, url, null);
        }

        // LIKES - End

        // FOLLOWERS
        function followMusicAlbum(albumId) {

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/' + albumId + '/follow';

            return requester.post(headers, url, null);
        }

        function unfollowMusicAlbum(albumId) {

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/' + albumId + '/follow';

            return requester.remove(headers, url, null);
        }

        // FOLLOWERS - End

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
            likeMusicAlbum: likeMusicAlbum,
            unlikeMusicAlbum: unlikeMusicAlbum,

            // Followers
            followMusicAlbum: followMusicAlbum,
            unfollowMusicAlbum: unfollowMusicAlbum,

            addMusicAlbum: addMusicAlbum,
            addSong: addSong
        }
    });
});