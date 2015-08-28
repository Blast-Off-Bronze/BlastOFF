define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('userDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'account/';
        var registrationUrl = serviceUrl + 'register';
        var loginUrl = serviceUrl + 'login';
        var logoutUrl = serviceUrl + 'logout';

        function register(user) {

            var headers = new requestHeaders().get();
            var url = registrationUrl;

            return requester.post(headers, url, user);
        }

        function login(user) {

            var headers = new requestHeaders().get();
            var url = loginUrl;

            return requester.post(headers, url, user);
        }

        function logout() {

            var headers = new requestHeaders().get();
            var url = logoutUrl;

            return requester.post(headers, url);
        }

        return {
            // Authentication
            register: register,
            login: login,
            logout: logout
        }
    });
});