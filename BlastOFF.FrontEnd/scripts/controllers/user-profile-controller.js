define(['app', 'user-data-service', 'notification-service', 'storage-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('userProfileController',
            function ($scope, $routeParams, $location, $http, storageService, userDataService, notificationService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.currentUserProfile = {};

                $scope.blasts = [];

                userDataService.userProfile($routeParams.username)
                .then(function (response){
                    console.log(response);
                    $scope.currentUserProfile = response;

                    $scope.getBlasts();
                }, function (error){
                    console.log(error);
                });

                $scope.getBlasts = function(startPostId, pageSize) {
                    userDataService.getBlasts($routeParams.username, startPostId, pageSize)
                    .then(function (response) {
                        response.forEach(function (blast) {
                            $scope.blasts.push(blast);
                        });

                        console.log(response);
                    }, function (error) {
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
            });
    });