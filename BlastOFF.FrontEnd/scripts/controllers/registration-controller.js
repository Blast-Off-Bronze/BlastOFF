define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service', 'notification-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('registrationController',
            function ($scope, $location, storageService, escapeSpecialCharsService, userDataService, notificationService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.guest = {
                    username: 'user1',
                    password: 123456,
                    confirmPassword: 123456,
                    email: 'user1@abv.bg'
                };

                $scope.register = function (guestInfo) {

                    //var guest = escapeSpecialCharsService.escapeSpecialCharacters(guestInfo, false);

                    userDataService.register(guestInfo).then(
                        function (response) {

                            var sessionToken = response['token_type'] + ' ' + response['access_token'];
                            var username = response['userName'];

                            storageService.setSessionToken(sessionToken);

                            notificationService.alertSuccess(constants.SUCCESSFUL_REGISTRATION_MESSAGE + username + '.');

                            $scope.resetForm();
                        },
                        function (error) {

                            notificationService.alertError(error);

                        });
                };

                $scope.resetForm = function () {
                    $scope.guest = {};
                    $location.path('#/');
                };
            });
    });