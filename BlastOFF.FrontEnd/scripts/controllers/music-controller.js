define(['app', 'songUpload', 'coverImageUpload', 'storage-service', 'music-data-service', 'constants', 'notification-service'],

    function (app) {
        'use strict';

        app.controller('musicController',
            function ($scope, $rootScope, $routeParams, $location, $http, storageService, musicDataService, constants, notificationService) {

                $scope.isLogged = storageService.isLogged();
                $scope.requesterIsBusy = true;
                $scope.allMusicAlbums = [];
                $scope.followedMusicAlbums = [];
                $scope.defaultMusicAlbumCoverImage = constants.DEFAULT_MUSIC_ALBUM_COVER_IMAGE_URL;
                $scope.displayEditMusicAlbum = false;

                //$scope.musicAlbum = {
                //    title: 'MusicAlbum_01',
                //    coverImageData: null
                //};
                //
                //$scope.song = {
                //    title: 'Song_01',
                //    artist: 'Artist_01',
                //    fileDataUrl: '',
                //    musicAlbumId: musicAlbumId
                //    trackNumber: null,
                //    originalAlbumTitle: null,
                //    originalAlbumArtist: null,
                //    originalDate: null,
                //    genre: null,
                //    composer: null,
                //    publisher: null,
                //    bpm: null
                //};

                $scope.musicAlbumComment = {
                    content: '',
                    musicAlbumId: ''
                };

                $scope.songComment = {
                    content: '',
                    songId: ''
                };

                musicDataService.getAllMusicAlbums().then(
                    function (response) {
                        $scope.allMusicAlbums = response;
                        $scope.requesterIsBusy = false;
                    },
                    function (error) {
                        notificationService.alertError(error);
                        $scope.requesterIsBusy = false;
                    });

                $scope.getAllSongs = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    musicDataService.getAllSongs(albumId).then(
                        function (response) {

                            musicAlbum['songs'] = response;
                            musicAlbum['allSongsDisplayed'] = true;
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                // EDIT
                $scope.saveMusicAlbumChanges = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    musicDataService.updateMusicAlbum(albumId, musicAlbum).then(
                        function (response) {
                            musicAlbum = response;
                            $scope.displayEditMusicAlbum = false;
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                // DELETE
                $scope.deleteMusicAlbum = function (musicAlbum) {

                    var albumId = musicAlbum['id'];
                    var albumPosition = $scope.allMusicAlbums.indexOf(musicAlbum);

                    musicDataService.deleteMusicAlbum(albumId).then(
                        function (response) {

                            $scope.allMusicAlbums.splice(albumPosition, 1);
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.deleteSong = function (musicAlbum, song) {

                    var songId = song['id'];
                    var songPosition = musicAlbum['songs'].indexOf(song);

                    musicDataService.deleteSong(songId).then(
                        function (response) {

                            musicAlbum['songs'].splice(songPosition, 1);
                            musicAlbum['songsCount']--;
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };


                // LIKES
                $scope.likeMusicAlbum = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    musicDataService.likeMusicAlbum(albumId).then(
                        function (response) {

                            musicAlbum['isLiked'] = true;
                            musicAlbum['likesCount']++;

                            notificationService.alertSuccess(response);
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.unlikeMusicAlbum = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    musicDataService.unlikeMusicAlbum(albumId).then(
                        function (response) {

                            musicAlbum['isLiked'] = false;
                            musicAlbum['likesCount']--;

                            notificationService.alertSuccess(response);
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.likeSong = function (song) {

                    var songId = song['id'];

                    musicDataService.likeSong(songId).then(
                        function (response) {

                            song['isLiked'] = true;
                            song['likesCount']++;

                            notificationService.alertSuccess(response);
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.unlikeSong = function (song) {

                    var songId = song['id'];

                    musicDataService.unlikeSong(songId).then(
                        function (response) {

                            song['isLiked'] = false;
                            song['likesCount']--;

                            notificationService.alertSuccess(response);
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };
                // LIKES - End

                // FOLLOWERS
                $scope.followMusicAlbum = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    musicDataService.followMusicAlbum(albumId).then(
                        function (response) {

                            musicAlbum['isFollowed'] = true;
                            musicAlbum['followersCount']++;

                            notificationService.alertSuccess(response);
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.unfollowMusicAlbum = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    musicDataService.unfollowMusicAlbum(albumId).then(
                        function (response) {

                            musicAlbum['isFollowed'] = false;
                            musicAlbum['followersCount']--;

                            notificationService.alertSuccess(response);
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };
                // FOLLOWERS - End

                // MUSIC ALBUM COMMENTS
                $scope.addMusicAlbumComment = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    $scope.musicAlbumComment['musicAlbumId'] = albumId;

                    musicDataService.addMusicAlbumComment(albumId, $scope.musicAlbumComment).then(
                        function (response) {
                            musicAlbum['comments'].push(response);
                            musicAlbum['commentsCount']++;
                            $scope.resetMusicAlbumComment();
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.resetMusicAlbumComment = function () {
                    $scope.musicAlbumComment = {
                        content: '',
                        musicAlbumId: ''
                    };
                };

                // SONG COMMENTS
                $scope.addSongComment = function (song) {

                    var songId = song['id'];

                    $scope.songComment['songId'] = songId;

                    musicDataService.addSongComment(songId, $scope.songComment).then(
                        function (response) {
                            song['comments'].push(response);
                            song['commentsCount']++;
                            $scope.resetSongComment();
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.resetSongComment = function () {
                    $scope.songComment = {
                        content: '',
                        songId: ''
                    };
                };


                $scope.addMusicAlbum = function (musicAlbum) {

                    musicDataService.addMusicAlbum(musicAlbum).then(
                        function (response) {
                            $scope.allMusicAlbums.push(response);
                            notificationService.alertSuccess("Music album successfully created.")
                        },
                        function (error) {
                            notificationService.alertError(error);
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
                $scope.toggledEditMusicAlbum = function () {
                    $scope.displayEditMusicAlbum = !$scope.displayEditMusicAlbum;
                };

                $scope.showOwnAndFollowedAlbums = function (album) {
                    return album['isOwn'] == true || album['isFollowed'] == true;
                };

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

                        if (audios[i] == e.target) {

                        }
                    }
                }, true);
            });
    });