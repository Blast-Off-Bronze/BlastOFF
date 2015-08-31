define(['app', 'songUpload', 'storage-service', 'music-data-service'],

    function (app) {
        'use strict';

        app.controller('musicController',
            function ($scope, $location, storageService, musicDataService) {

                $scope.isLogged = storageService.isLogged();

                $scope.uploadSong = function (song) {

                    var uintArray = new Uint8Array(song);

                    console.log(uintArray);

                    musicDataService.addSong(uintArray).then(
                        function (response) {

                            console.log(response);

                        },
                        function (error) {

                            console.log(error);

                        });
                };
            });
    });