define(['app', 'blast-data-service', 'storage-service', 'notification-service'],

    function (app) {
        'use strict';

        app.controller('blastController',
            function ($scope, $routeParams, blastDataService, storageService, notificationService) {

                $scope.isLogged = storageService.isLogged();

                $scope.noBlastsAvailable = false;
                $scope.gettingBlasts = false;
                $scope.previousBlastsExist = false;
                $scope.allBlasts = [];

                //blastDataService.getAllBlasts()
                //.then(function(response) {
                //    $scope.allBlasts = response;
                //    console.log(response);
                //}, function(error) {
                //    console.log(error);
                //});

                // PAGED BLASTS
                $scope.getPagedBlasts = function (startBlastId, pageSize) {
                    if ($scope.gettingBlasts) {
                        return;
                    } else {
                        $scope.gettingBlasts = true;

                        blastDataService.getPagedBlasts("blqblq", 3, pageSize).then(
                            function (response) {
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