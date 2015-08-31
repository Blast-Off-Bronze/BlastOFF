define(['app', 'songUpload', 'storage-service'],

    function (app) {
        'use strict';

        app.controller('musicController',
            function ($scope, $location, storageService) {

                $scope.isLogged = storageService.isLogged();
                $scope.song = "";

                $scope.uploadSong = function (song) {

                    console.log(song);

                };
            });
    });