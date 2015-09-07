define(['app', 'constants'], function (app) {
    'use strict';

    app.factory('redirectionService', function ($timeout, $location, constants) {

        function redirect() {
            $timeout(function () {
                $location.path('#/');
            }, constants.DEFAULT_REDIRECTION_TIMEOUT);
        }

        return {
            redirect: redirect
        }
    });
});