define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service', 'notification-service', 'constants', 'music-data-service'],

    function (app) {
        'use strict';

        app.controller('homeController',
            function ($scope, $location, $rootScope, $sce, storageService, escapeSpecialCharsService, userDataService, notificationService, constants, musicDataService) {

                $scope.isLogged = storageService.isLogged();

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