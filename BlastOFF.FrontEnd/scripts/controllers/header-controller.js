define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service', 'notification-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('headerController',
            function ($scope, $location, storageService, escapeSpecialCharsService, userDataService, notificationService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.logout = function () {
                    userDataService.logout()
                        .then(function (response) {
                            storageService.clearStorage();
                            $scope.isLogged = storageService.isLogged();
                            notificationService.alertSuccess(response['message']);
                            $location.path('#/');
                        }, function (error) {
                            notificationService.alertError(error);
                        });
                };

                $scope.resetForm = function () {
                    $scope.guest = {};
                    $location.path('#/');
                };

                //Event Listeners
                $scope.$on('currentUserDataUpdated', function () {
                    $scope.isLogged = storageService.isLogged();
                });
            });
    });