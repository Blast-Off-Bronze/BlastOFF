define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service',
 'notification-service', 'constants', 'music-data-service', 'blast-data-service'],

    function (app) {
        'use strict';

        app.controller('homeController',
            function ($scope, $location, $rootScope, $sce, storageService, escapeSpecialCharsService,
             userDataService, notificationService, constants, musicDataService,
             blastDataService) {

                $scope.isLogged = storageService.isLogged();

                $scope.noBlastsAvailable = false;
                $scope.noImageAlbumsAvailable = false;
                $scope.gettingBlasts = false;
                $scope.gettingImageAlbums = false;
                $scope.currentPageImageAlbums = 0;
                $scope.currentPageBlasts = 0;

                $scope.blasts = [];
                $scope.imageAlbums = [];

                $scope.blastToPost = {};

                getPublicBlasts();

                function getPublicBlasts(currentPage, pageSize) {
                    blastDataService.getPublicBlasts(currentPage, pageSize)
                    .then(function (response) {
                        if(response.length < 1) {
                            $scope.noBlastsAvailable = true;
                        } else {
                            $scope.noBlastsAvailable = false;
                            response.forEach(function (blast) {
                                $scope.blasts.push(blast);
                            });

                            $scope.currentPageBlasts += 1;
                        }

                        $scope.gettingBlasts = false;
                        console.log(response);
                    }, function (error) {
                        $scope.noBlastsAvailable = false;
                        console.log(error);
                        notificationService.alertError(error);
                    });
                }

                $scope.getBlasts = function (currentPage, pageSize) {
                    getPublicBlasts(currentPage, pageSize);
                };

                $scope.makeABlast = function () {
                    userDataService.makeABlast($scope.blastToPost)
                    .then(function(response) {
                        $scope.blasts.unshift(response);
                        console.log(response);
                    }, function(error) {
                        console.log(error);
                    });
                };

                $scope.setFilters = function (event) {
                    event.preventDefault();
                };
            });
    });