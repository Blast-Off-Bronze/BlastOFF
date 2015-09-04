define(['app', 'storage-service', 'escape-special-chars-service', 'user-data-service', 'notification-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('loginController',
            function ($scope, $location, storageService, escapeSpecialCharsService, userDataService, notificationService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.guest = {
                    username: "",
                    password: "",
                    wantsToBeRemembered: false
                };

                $scope.login = function (guestInfo) {

                    var guest = escapeSpecialCharsService.escapeSpecialCharacters(guestInfo, false);

                    userDataService.login(guest).then(
                        function (response) {

                            var sessionToken = response['token_type'] + ' ' + response['access_token'];
                            var username = response['userName'];

                            if (guest.wantsToBeRemembered) {
                                storageService.setSessionToken(sessionToken, true);
                            } else {
                                storageService.setSessionToken(sessionToken, false);
                            }

                            notificationService.alertSuccess(constants.SUCCESSFUL_LOGIN_MESSAGE + username + '.');

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