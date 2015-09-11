define(['app', 'songUpload', 'coverImageUpload', 'storage-service', 'music-data-service', 'constants', 'notification-service'],

    function (app) {
        'use strict';

        app.controller('musicController',
            function ($scope, $rootScope, $routeParams, $location, $http, storageService, musicDataService, constants, notificationService) {

                $scope.isLogged = storageService.isLogged();
                $scope.uploadingSong = false;
                $scope.requesterIsBusy = true;
                $scope.allMusicAlbums = [];
                $scope.defaultMusicAlbumCoverImage = constants.DEFAULT_MUSIC_ALBUM_COVER_IMAGE_URL;

                $scope.noMusicAlbumsAvailable = false;
                $scope.gettingMusicAlbums = false;
                $scope.currentPage = 0;

                $scope.newMusicAlbum = {
                    title: '',
                    coverImageData: null
                };

                $scope.newSong = {
                    title: '',
                    artist: '',
                    fileDataUrl: '',
                    // Optional
                    trackNumber: null,
                    originalAlbumTitle: null,
                    originalAlbumArtist: null,
                    originalDate: null,
                    genre: null,
                    composer: null,
                    publisher: null,
                    bpm: null
                };

                $scope.musicAlbumComment = {
                    content: '',
                    musicAlbumId: ''
                };

                $scope.songComment = {
                    content: '',
                    songId: ''
                };

                //$scope.getPagedMusicAlbums = function (currentPage, pageSize) {
                //
                //    $scope.gettingMusicAlbums = true;
                //
                //    musicDataService.getPagedMusicAlbums(currentPage, pageSize)
                //        .then(function (response) {
                //            if(response.length < 1) {
                //                $scope.noMusicAlbumsAvailable = true;
                //            } else {
                //                $scope.noMusicAlbumsAvailable = false;
                //                response.forEach(function (musicAlbum) {
                //                    $scope.allMusicAlbums.push(musicAlbum);
                //                });
                //
                //                $scope.currentPage += pageSize || 3;
                //            }
                //
                //            $scope.gettingMusicAlbums = false;
                //
                //            console.log(response);
                //        }, function (error) {
                //            $scope.noMusicAlbumsAvailable = false;
                //            console.log(error);
                //        });
                //};

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

                // ADD
                $scope.addMusicAlbum = function (musicAlbum) {

                    musicAlbum['isPublic'] = true;

                    musicDataService.addMusicAlbum(musicAlbum).then(
                        function (response) {
                            $scope.allMusicAlbums.unshift(response);
                            $scope.newMusicAlbum = {
                                title: '',
                                coverImageData: null
                            };
                            notificationService.alertSuccess("Music album successfully created.")
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.addSong = function (musicAlbum, song) {

                    $scope.uploadingSong = true;

                    var albumId = musicAlbum['id'];
                    musicAlbum['displayAddSong'] = true;

                    musicDataService.addSong(albumId, song).then(
                        function (response) {

                            $scope.uploadingSong = false;

                            musicAlbum['songs'].unshift(response);

                            $scope.newSong = {
                                title: '',
                                artist: '',
                                fileDataUrl: null,
                                trackNumber: null,
                                originalAlbumTitle: null,
                                originalAlbumArtist: null,
                                originalDate: null,
                                genre: null,
                                composer: null,
                                publisher: null,
                                bpm: null
                            };
                            notificationService.alertSuccess("Song successfully added to album.");


                            musicAlbum['displayAddSong'] = false;
                        },
                        function (error) {
                            $scope.uploadingSong = false;
                            notificationService.alertError(error);
                        });
                };

                // EDIT
                $scope.saveMusicAlbumChanges = function (musicAlbum) {

                    var albumId = musicAlbum['id'];
                    musicAlbum['displayEditMusicAlbum'] = '';

                    musicDataService.updateMusicAlbum(albumId, musicAlbum).then(
                        function (response) {
                            musicAlbum = response;
                            musicAlbum['displayEditMusicAlbum'] = false;
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.saveSongChanges = function (song) {

                    var songId = song['id'];
                    song['displayEditSong'] = '';

                    musicDataService.updateSong(songId, song).then(
                        function (response) {
                            song = response;
                            song['displayEditSong'] = false;
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
                $scope.getMusicAlbumComments = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    musicDataService.getAllMusicAlbumComments(albumId).then(
                        function (response) {
                            musicAlbum['comments'] = response;
                            musicAlbum['allCommentsDisplayed'] = true;
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.addMusicAlbumComment = function (musicAlbum) {

                    var albumId = musicAlbum['id'];

                    $scope.musicAlbumComment['musicAlbumId'] = albumId;

                    musicDataService.addMusicAlbumComment(albumId, $scope.musicAlbumComment).then(
                        function (response) {
                            musicAlbum['comments'].unshift(response);
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
                $scope.getSongComments = function (song) {

                    var songId = song['id'];

                    musicDataService.getAllSongComments(songId).then(
                        function (response) {
                            song['comments'] = response;
                            song['allCommentsDisplayed'] = true;
                        },
                        function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.addSongComment = function (song) {

                    var songId = song['id'];

                    $scope.songComment['songId'] = songId;

                    musicDataService.addSongComment(songId, $scope.songComment).then(
                        function (response) {
                            song['comments'].unshift(response);
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

                // MISCELLANEOUS FUNCTIONS
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