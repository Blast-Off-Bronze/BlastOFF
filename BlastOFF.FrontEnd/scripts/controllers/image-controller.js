define(['app', 'coverImageUpload', 'storage-service', 'image-data-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('imageController',
            function ($scope, $routeParams, $location, $http, storageService, imageDataService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.imageAlbumModel = {};
                $scope.imageModel = {};

                $scope.imageAlbums = [];

                imageDataService.getMyAlbums()
                .then(function(response){
                    $scope.myImageAlbums = response;
                    console.log(response);
                }, function(error){
                    console.log(error);
                });

                $scope.addImageAlbum = function (imageAlbum) {
                    imageDataService.addImageAlbum(imageAlbum).then(
                        function (response) {
                            console.log(response);
                            $scope.myImageAlbums.push(response);
                        },
                        function (error) {

                            console.log(error);

                        });
                };

                $scope.chooseAlbum = function (imageAlbumId) {
                    $scope.image.imageAlbumId = imageAlbumId;
                    console.log(imageAlbumId);
                };

                $scope.commentImageAlbum = function(imageAlbum) {
                    imageDataService.commentImageAlbum(imageAlbum)
                    .then(function (response) {
                        blast.comments.unshift(response);
                        blast.commentModel.content = '';
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.addImage = function (image) {
                    console.log(image);
                    imageDataService.addImage(image).then(
                        function (response) {
                            console.log(response);
                        },
                        function (error) {
                            console.log(error);
                        });
                };
            });
    });