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
                $scope.gettingBlasts = false;
                $scope.previousBlastsExist = false;
                $scope.blasts = [];

                $scope.blastToPost = {};

                getPublicBlasts();

                function getPublicBlasts(startBlastId, pageSize) {
                    blastDataService.getPublicBlasts(startBlastId, pageSize)
                    .then(function (response) {
                        response.forEach(function (blast) {
                            $scope.blasts.push(blast);
                        });
                        console.log(response);
                    }, function (error) {
                        console.log(error);
                        notificationService.alertError(error);
                    });
                }

                $scope.getBlasts = function (startBlastId, pageSize) {
                    getPublicBlasts(startBlastId, pageSize);
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
            });
    });