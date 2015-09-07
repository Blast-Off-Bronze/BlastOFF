define(['app', 'coverImageUpload', 'storage-service', 'image-data-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('imageController',
            function ($scope, $routeParams, $location, $http, storageService, imageDataService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.imageAlbum = {
                    title: 'test',
                };

                imageDataService.getMyAlbums()
                .then(function(response){
                    $scope.myImageAlbums = response;
                    console.log(response);
                }, function(error){
                    console.log(error);
                });

                $scope.image = {
                    title: '',
                    base64ImageString: '',
                    imageAlbumId: ''
                };

                $scope.addImageAlbum = function (imageAlbum) {

                    console.log(imageAlbum);

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