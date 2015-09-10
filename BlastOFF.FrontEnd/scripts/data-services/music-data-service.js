define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('musicDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'music/albums';
        var songsUrl = constants.BASE_URL + 'songs';

        function getPagedMusicAlbums(currentPage, pageSize) {

            var currentPage = currentPage || constants.DEFAULT_CURRENT_PAGE;
            var pageSize = pageSize || 3;

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/?CurrentPage=' + currentPage + '&PageSize=' + pageSize;

            return requester.get(headers, url);
        }

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

        function updateSong(songId, song) {

            var headers = new requestHeaders().get();
            var url = songsUrl + '/' + songId;

            return requester.put(headers, url, song);
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
        function getAllSongComments(songId) {

            var headers = new requestHeaders().get();

            var url = songsUrl + '/' + songId + '/comments';

            return requester.get(headers, url);
        }

        function getAllMusicAlbumComments(albumId) {

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/' + albumId + '/comments';

            return requester.get(headers, url);
        }

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
            getPagedMusicAlbums: getPagedMusicAlbums,
            getAllMusicAlbums: getAllMusicAlbums,
            getAllSongs: getAllSongs,

            // Edit
            updateMusicAlbum: updateMusicAlbum,
            updateSong: updateSong,

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
            getAllMusicAlbumComments: getAllMusicAlbumComments,
            getAllSongComments: getAllSongComments,
            addMusicAlbumComment: addMusicAlbumComment,
            addSongComment: addSongComment,

            addMusicAlbum: addMusicAlbum,
            addSong: addSong
        }
    });
});