define(['app', 'songUpload', 'coverImageUpload', 'storage-service', 'music-data-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('musicController',
            function ($scope, $routeParams, $location, $http, storageService, musicDataService, constants) {

                $scope.isLogged = storageService.isLogged();

                var musicAlbumId = $routeParams.musicAlbumId;

                $scope.musicAlbum = {
                    title: 'MusicAlbum_01',
                    coverImageData: ''
                };

                $scope.song = {
                    title: 'Song_01',
                    artist: 'Artist_01',
                    fileDataUrl: '',
                    musicAlbumId: musicAlbumId
                    //trackNumber: null,
                    //originalAlbumTitle: null,
                    //originalAlbumArtist: null,
                    //originalDate: null,
                    //genre: null,
                    //composer: null,
                    //publisher: null,
                    //bpm: null
                };

                $scope.addMusicAlbum = function (musicAlbum) {

                    console.log(musicAlbum);

                    musicDataService.addMusicAlbum(musicAlbum).then(
                        function (response) {

                            console.log(response);

                        },
                        function (error) {

                            console.log(error);

                        });
                };

                $scope.addSong = function (song) {

                    console.log(song);

                    musicDataService.addSong(musicAlbumId, song).then(
                        function (response) {

                            console.log(response);

                        },
                        function (error) {

                            console.log(error);

                        });
                };
            });
    });