define(['app', 'user-data-service'],

    function (app) {
        'use strict';

        app.controller('registrationController',
            function ($scope, $location, userDataService) {

                $scope.guest = {
                    username: "testUser1",
                    password: "12345a",
                    confirmPassword: "12345a",
                    email: "user1@gmail.com"
                };

                $scope.register = function (guestInfo) {

                    var guest = guestInfo;

                    userDataService.register(guest).then(
                        function (response) {

                            console.log(response);

                        },
                        function (error) {

                            console.log(error);

                        });
                };
            });
    });