define(['app', 'user-data-service', 'notification-service', 'storage-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('userProfileController',
            function ($scope, $routeParams, $location, $http, storageService, userDataService, notificationService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.currentUserProfile = {};

                $scope.blasts = [];

                $scope.imageAlbums = [];

                $scope.noBlastsAvailable = false;
                $scope.noImageAlbumsAvailable = false;
                $scope.gettingBlasts = false;
                $scope.gettingImageAlbums = false;
                $scope.currentPageImageAlbums = 0;
                $scope.currentPageBlasts = 0;

                userDataService.userProfile($routeParams.username)
                .then(function (response){
                    console.log(response);
                    $scope.currentUserProfile = response;

                    $scope.getBlasts();
                }, function (error){
                    console.log(error);
                });

                $scope.getBlasts = function(currentPage, pageSize) {
                        $scope.gettingBlasts = true;

                        userDataService.getBlasts($routeParams.username, currentPage, pageSize)
                        .then(function (response) {
                            if(response.length < 1) {
                                $scope.noBlastsAvailable = true;
                            } else {
                                $scope.noBlastsAvailable = false;
                                response.forEach(function (blast) {
                                    $scope.blasts.push(blast);
                                });

                                $scope.currentPageBlasts += pageSize || constants.DEFAULT_BLAST_FEED_PAGE_SIZE;
                            }

                            $scope.gettingBlasts = false;

                            console.log(response);
                        }, function (error) {
                            $scope.noBlastsAvailable = false;
                            console.log(error);
                        });
                };

                $scope.getImageAlbums = function(currentPage, pageSize) {
                    $scope.gettingImageAlbums = true;

                    userDataService.getImageAlbums($routeParams.username, currentPage, pageSize)
                    .then(function (response) {
                        if(response.length < 1) {
                            $scope.noImageAlbumsAvailable = true;
                        } else {
                            $scope.noImageAlbumsAvailable = false;
                            response.forEach(function (imageAlbum) {
                                $scope.imageAlbums.push(imageAlbum);
                            });

                            $scope.currentPageImageAlbums += pageSize || constants.DEFAULT_BLAST_FEED_PAGE_SIZE;
                        }

                        $scope.gettingImageAlbums = false;

                        console.log(response);
                    }, function (error) {
                        $scope.noImageAlbumsAvailable = false;
                        console.log(error);
                    });
                };

                $scope.follow = function() {
                    userDataService.followUser($scope.currentUserProfile.username)
                    .then(function (response){
                        console.log(response);
                        $scope.currentUserProfile.followedByMe = true;
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.unfollow = function() {
                    userDataService.unfollowUser($scope.currentUserProfile.username)
                    .then(function (response){
                        console.log(response);
                        $scope.currentUserProfile.followedByMe = false;
                    }, function (error) {
                        console.log(error);
                    });
                };

                $scope.setFilters = function (event) {
                    event.preventDefault();
                };
            });
    });