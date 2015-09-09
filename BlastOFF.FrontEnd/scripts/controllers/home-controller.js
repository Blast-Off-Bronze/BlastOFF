define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service',
 'notification-service', 'constants', 'music-data-service', 'blast-data-service'],

    function (app) {
        'use strict';

        app.controller('homeController',
            function ($scope, $location, $rootScope, $sce, storageService, escapeSpecialCharsService,
             userDataService, notificationService, constants, musicDataService,
             blastDataService) {

                $scope.isLogged = storageService.isLogged();

                 $scope.noBlastsAvailable = false;
                 $scope.gettingBlasts = false;
                 $scope.previousBlastsExist = false;
                 $scope.allBlasts = [];

                $scope.blastToPost = {};

                $scope.getPagedBlasts = function (startBlastId, pageSize) {
                    if ($scope.gettingBlasts) {
                        return;
                    } else {
                        $scope.gettingBlasts = true;

                        blastDataService.getPublicBlasts(0, 3)
                        .then(function (response) {
                                if (response.length < 1) {
                                    $scope.noBlastsAvailable = true;
                                } else {
                                    $scope.allBlasts = response;
                                    $scope.previousBlasts = true;
                                }
                                $scope.gettingBlasts = false;
                            },
                            function (error) {
                                $scope.gettingBlasts = false;
                                console.log(error);
                                notificationService.alertError(error);
                            });
                    }
                };

                $scope.makeABlast = function () {
                console.log($scope.blastToPost);
                    userDataService.makeABlast($scope.blastToPost)
                    .then(function(response) {
                        console.log(response);
                    }, function(error) {
                        console.log(error);
                    });
                };

                // MUSIC
                $scope.requesterIsBusy = false;
                $scope.allMusicAlbums = [];
                $scope.defaultMusicAlbumCoverImage = constants.DEFAULT_MUSIC_ALBUM_COVER_IMAGE_URL;

                document.addEventListener('play', function(e){
                    var audios = document.getElementsByTagName('audio');
                    for(var i = 0, len = audios.length; i < len;i++){
                        if(audios[i] != e.target){
                            audios[i].pause();
                            audios[i].currentTime = 0;
                        }
                    }
                }, true);

                $scope.getAllMusicAlbums = function () {
                    $scope.requesterIsBusy = true;

                    musicDataService.getAllMusicAlbums().then(
                        function (response) {
                            $scope.allMusicAlbums = response;
                            $scope.requesterIsBusy = false;
                        },
                        function (error) {
                            $scope.requesterIsBusy = false;
                            notificationService.alertError(error);
                        });
                };

            });
    });