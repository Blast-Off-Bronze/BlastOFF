define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('musicDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'music/albums';
        var songsUrl = constants.BASE_URL + 'songs';

        function getAllMusicAlbums() {

            var headers = new requestHeaders().get();

            return requester.get(headers, serviceUrl);
        }

        function getAllSongs(albumId) {

            var headers = new requestHeaders().get();
            var url = serviceUrl + '/' + albumId + '/songs';

            return requester.get(headers, url);
        }

        // EDIT
        function updateMusicAlbum(albumId, musicAlbum) {

            var headers = new requestHeaders().get();
            var url = serviceUrl + '/' + albumId;

            return requester.put(headers, url, musicAlbum);
        }

        // DELETE
        function deleteMusicAlbum(albumId) {

            var headers = new requestHeaders().get();
            var url = serviceUrl + '/' + albumId;

            return requester.remove(headers, url);
        }

        function deleteSong(songId) {

            var headers = new requestHeaders().get();
            var url = songsUrl + '/' + songId;

            return requester.remove(headers, url);
        }

        // DELETE - End

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

        function likeSong(songId) {

            var headers = new requestHeaders().get();

            var url = songsUrl + '/' + songId + '/likes';

            return requester.post(headers, url, null);
        }

        function unlikeSong(songId) {

            var headers = new requestHeaders().get();

            var url = songsUrl + '/' + songId + '/likes';

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

        // COMMENTS
        function addMusicAlbumComment(albumId, musicAlbumComment) {

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/' + albumId + '/comments';

            return requester.post(headers, url, musicAlbumComment);
        }

        function addSongComment(songId, songComment) {

            var headers = new requestHeaders().get();

            var url = songsUrl + '/' + songId + '/comments';

            return requester.post(headers, url, songComment);
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

            // Edit
            updateMusicAlbum: updateMusicAlbum,

            // Delete
            deleteMusicAlbum: deleteMusicAlbum,
            deleteSong: deleteSong,

            // Likes
            likeMusicAlbum: likeMusicAlbum,
            unlikeMusicAlbum: unlikeMusicAlbum,
            likeSong: likeSong,
            unlikeSong: unlikeSong,

            // Followers
            followMusicAlbum: followMusicAlbum,
            unfollowMusicAlbum: unfollowMusicAlbum,

            // Comments
            addMusicAlbumComment: addMusicAlbumComment,
            addSongComment: addSongComment,

            addMusicAlbum: addMusicAlbum,
            addSong: addSong
        }
    });
});