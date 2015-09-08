define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service', 'notification-service', 'constants', 'music-data-service'],

    function (app) {
        'use strict';

        app.controller('homeController',
            function ($scope, $location, $rootScope, storageService, escapeSpecialCharsService, userDataService, notificationService, constants, musicDataService) {

                $scope.isLogged = storageService.isLogged();

                // MUSIC
                $scope.requesterIsBusy = false;
                $scope.allMusicAlbums = [];
                $scope.defaultMusicAlbumCoverImage = constants.DEFAULT_MUSIC_ALBUM_COVER_IMAGE_URL;

                $scope.getAllMusicAlbums = function () {
                    $scope.requesterIsBusy = true;

                    musicDataService.getAllMusicAlbums().then(
                        function (response) {
                            console.log(response)
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