define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service', 'notification-service', 'constants', 'music-data-service'],

    function (app) {
        'use strict';

        app.controller('homeController',
            function ($scope, $location, $rootScope, storageService, escapeSpecialCharsService, userDataService, notificationService, constants, musicDataService) {

                $scope.isLogged = storageService.isLogged();

                // MUSIC
                $scope.requesterIsBusy = false;
                $scope.allMusicAlbums = [];

                $scope.getAllMusicAlbums = function () {
                    $scope.requesterIsBusy = true;

                    musicDataService.getAllMusicAlbums().then(
                        function (response) {
                            $scope.allMusicAlbums = response;

                            $scope.allMusicAlbums.forEach(function (musicAlbum) {
                                if (musicAlbum['coverImageData'] == null) {
                                    musicAlbum['coverImageData'] = constants.DEFAULT_MUSIC_ALBUM_COVER_IMAGE_URL;
                                }
                            });

                            $scope.requesterIsBusy = false;
                        },
                        function (error) {
                            $scope.requesterIsBusy = false;
                            notificationService.alertError(error);
                        });
                };


            });
    });