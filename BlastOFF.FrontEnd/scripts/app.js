define(['angularAMD', 'angular-messages', 'angular-route', 'infiniteScroll'], function (angularAMD) {
    'use strict';

    var app = angular.module('app', ['ngMessages', 'ngRoute', 'infinite-scroll']);

    app.config(function ($routeProvider, $locationProvider) {

        $routeProvider
            .when('/', angularAMD.route({
                templateUrl: 'templates/home.html',
                controller: 'homeController',
                controllerUrl: 'controllers/home-controller'
            }));

        // Registration
        $routeProvider
            .when('/registration', angularAMD.route({
                templateUrl: 'templates/registration.html',
                controller: 'registrationController',
                controllerUrl: 'controllers/registration-controller'
            }));

        // Login
        $routeProvider
            .when('/login', angularAMD.route({
                templateUrl: 'templates/login.html',
                controller: 'loginController',
                controllerUrl: 'controllers/login-controller'
            }));

        $routeProvider
            .when('/image', angularAMD.route({
                templateUrl: 'templates/image.html',
                controller: 'imageController',
                controllerUrl: 'controllers/image-controller'
            }));

         $routeProvider
            .when('/blast-feed', angularAMD.route({
                templateUrl: 'templates/blast-feed.html',
                controller: 'blastController',
                controllerUrl: 'controllers/blast-controller'
            }));

        $routeProvider
            .when('/users/:username', angularAMD.route({
                templateUrl: 'templates/user-profile.html',
                controller: 'userProfileController',
                controllerUrl: 'controllers/user-profile-controller'
            }));

        //// Add music album
        //$routeProvider
        //    .when('/music/albums', angularAMD.route({
        //        templateUrl: 'templates/music.html',
        //        controller: 'musicController',
        //        controllerUrl: 'controllers/music-controller'
        //    }));
        //
        //// Add song
        //$routeProvider
        //    .when('/music/albums/:musicAlbumId/songs', angularAMD.route({
        //        templateUrl: 'templates/music.html',
        //        controller: 'musicController',
        //        controllerUrl: 'controllers/music-controller'
        //    }));

        //// Change password
        //$routeProvider
        //    .when('/profile/password', angularAMD.route({
        //        templateUrl: 'templates/change-password.html',
        //        controller: 'changePasswordController',
        //        controllerUrl: 'controllers/change-password-controller'
        //    }));
        //
        //// Edit Profile
        //$routeProvider
        //    .when('/profile', angularAMD.route({
        //        templateUrl: 'templates/edit-profile.html',
        //        controller: 'editProfileController',
        //        controllerUrl: 'controllers/edit-profile-controller'
        //    }));
        //
        //// Friends
        //$routeProvider
        //    .when('/users/:username/friends', angularAMD.route({
        //        templateUrl: 'templates/friends.html',
        //        controller: 'friendsController',
        //        controllerUrl: 'controllers/friends-controller'
        //    }));
        //
        //// Home Page
        //$routeProvider
        //    .when('/', angularAMD.route({
        //        templateUrl: 'templates/home-page.html',
        //        controller: 'homePageController',
        //        controllerUrl: 'controllers/home-page-controller'
        //    }));
        //
        //
        //// Logout
        //$routeProvider
        //    .when('/logout', angularAMD.route({
        //        templateUrl: 'templates/logout.html',
        //        controller: 'logoutController',
        //        controllerUrl: 'controllers/logout-controller'
        //    }));
        //
        //// Wall
        //$routeProvider
        //    .when('/users/:username', angularAMD.route({
        //        templateUrl: 'templates/wall.html',
        //        controller: 'wallController',
        //        controllerUrl: 'controllers/wall-controller'
        //    }));

        $routeProvider.otherwise({redirectTo: '/'});

        //$locationProvider.html5Mode(true);
    });

    return angularAMD.bootstrap(app);
});