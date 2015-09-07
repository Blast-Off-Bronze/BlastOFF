define(['app', 'blast-data-service', 'storage-service'],

    function (app) {
        'use strict';

        app.controller('blastController',
            function ($scope, $routeParams, blastDataService, storageService) {

                $scope.isLogged = storageService.isLogged();

                blastDataService.getAllBlasts()
                .then(function(response) {
                    $scope.allBlasts = response;
                    console.log(response);
                }, function(error) {
                    console.log(error);
                });

//                $scope.imageAlbum = {
//                    title: '',
//                };
//
//                imageDataService.getMyAlbums()
//                .then(function(response){
//                    $scope.myImageAlbums = response;
//                    console.log(response);
//                }, function(error){
//                    console.log(error);
//                });
//
//                $scope.image = {
//                    title: '',
//                    base64ImageString: '',
//                    imageAlbumId: ''
//                };
//
//                $scope.addImageAlbum = function (imageAlbum) {
//
//                    console.log(imageAlbum);
//
//                    imageDataService.addImageAlbum(imageAlbum).then(
//                        function (response) {
//
//                            console.log(response);
//                            $scope.myImageAlbums.push(response);
//                        },
//                        function (error) {
//
//                            console.log(error);
//
//                        });
//                };
//
//                $scope.chooseAlbum = function (imageAlbumId) {
//                    $scope.image.imageAlbumId = imageAlbumId;
//                    console.log(imageAlbumId);
//                };
//
//                $scope.addImage = function (image) {
//
//                    console.log(image);
//
//                    imageDataService.addImage(image).then(
//                        function (response) {
//
//                            console.log(response);
//
//                        },
//                        function (error) {
//
//                            console.log(error);
//
//                        });
//                };
            });
    });