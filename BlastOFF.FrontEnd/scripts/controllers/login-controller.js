define(['app', 'user-data-service'],

    function (app) {
        'use strict';

        app.controller('loginController',
            function ($scope, $location, userDataService) {

                $scope.guest = {
                    username: "testUser1",
                    password: "12345a"
                };

                $scope.login = function (guestInfo) {

                    var guest = guestInfo;

                    userDataService.login(guest).then(
                        function (response) {

                            console.log(response);

                        },
                        function (error) {

                            console.log(error);

                        });
                };
            });
    });