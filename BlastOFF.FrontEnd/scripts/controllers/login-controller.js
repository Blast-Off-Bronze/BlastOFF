define(['app', 'storage-service', 'redirection-service', 'escape-special-chars-service', 'user-data-service', 'notification-service', 'constants'],

    function (app) {
        'use strict';

        app.controller('loginController',
            function ($scope, $location, $rootScope, redirectionService, storageService, escapeSpecialCharsService, userDataService, notificationService, constants) {

                $scope.isLogged = storageService.isLogged();

                $scope.guest = {
                    username: 'user1',
                    password: 123456,
                    wantsToBeRemembered: false
                };

                $scope.login = function (guestInfo) {

                   // var guest = escapeSpecialCharsService.escapeSpecialCharacters(guestInfo, false);

                    userDataService.login(guestInfo).then(
                        function (response) {

                            var sessionToken = response['token_type'] + ' ' + response['access_token'];
                            var username = response['userName'];

                            var userDetails = {
                                username: username
                            };

                            if (guestInfo.wantsToBeRemembered) {
                                storageService.setSessionToken(sessionToken, true);
                                storageService.setUserDetails(userDetails, true);
                            } else {
                                storageService.setSessionToken(sessionToken, false);
                                storageService.setUserDetails(userDetails, false);
                            }

                            notificationService.alertSuccess(constants.SUCCESSFUL_LOGIN_MESSAGE + username + '.');

                            $scope.resetForm();

                            $rootScope.$broadcast('currentUserDataUpdated');
                        },
                        function (error) {

                            notificationService.alertError(error);

                        });
                };

                $scope.resetForm = function () {
                    $scope.guest = {};
                    $location.path('#/');
                };

                $scope.redirect = function () {
                    redirectionService.redirect();
                };
            });
    });