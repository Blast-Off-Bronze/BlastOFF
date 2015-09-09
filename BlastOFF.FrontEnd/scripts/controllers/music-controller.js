define(['app', 'songUpload', 'coverImageUpload', 'storage-service', 'music-data-service', 'constants', 'notification-service'],

    function (app) {
        'use strict';

        app.controller('musicController',
            function ($scope, $rootScope, $routeParams, $location, $http, storageService, musicDataService, constants, notificationService) {

                $scope.isLogged = storageService.isLogged();
                $scope.requesterIsBusy = false;
                $scope.allMusicAlbums = [];
                $scope.followedMusicAlbums = [];
                $scope.defaultMusicAlbumCoverImage = constants.DEFAULT_MUSIC_ALBUM_COVER_IMAGE_URL;

                var musicAlbumId = $routeParams.musicAlbumId;

                $scope.musicAlbum = {
                    title: 'MusicAlbum_01',
                    coverImageData: null
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

                musicDataService.getAllMusicAlbums().then(
                    function (response) {
                        console.log(response);
                        $scope.allMusicAlbums = response;
                    },
                    function (error) {
                        notificationService.alertError(error);
                    });

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


                // MISCELLANEOUS FUNCTIONS
                $scope.setFilters = function (event) {
                    event.preventDefault();
                };

                document.addEventListener('play', function (e) {
                    var audios = document.getElementsByTagName('audio');
                    for (var i = 0, len = audios.length; i < len; i++) {
                        if (audios[i] != e.target) {
                            audios[i].pause();
                            audios[i].currentTime = 0;
                        }
                    }
                }, true);
            });
    });