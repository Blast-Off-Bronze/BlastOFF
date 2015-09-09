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
                 $scope.allBlasts = [];

                $scope.blastToPost = {};

                $scope.getPagedBlasts = function (startBlastId, pageSize) {
                    if ($scope.gettingBlasts) {
                        return;
                    } else {
                        $scope.gettingBlasts = true;

                        blastDataService.getPublicBlasts(0, 3)
                        .then(function (response) {
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

                $scope.makeABlast = function () {
                console.log($scope.blastToPost);
                    userDataService.makeABlast($scope.blastToPost)
                    .then(function(response) {
                        console.log(response);
                    }, function(error) {
                        console.log(error);
                    });
                };
            });
    });