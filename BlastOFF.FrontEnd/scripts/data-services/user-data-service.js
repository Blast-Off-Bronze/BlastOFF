define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('userDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'account/';
        var registrationUrl = serviceUrl + 'register';
        var loginUrl = serviceUrl + 'login';
        var logoutUrl = serviceUrl + 'logout';

        var usersServiceUrl = constants.BASE_URL + 'users/';

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

        function userProfile(username) {
            var headers = new requestHeaders().get();

            return requester.get(headers, usersServiceUrl + username + '/profile');
        }

        function userFollowers(username) {
            var headers = new requestHeaders().get();

            return requester.get(headers, usersServiceUrl + username + '/followers');
        }

        function followUser(username) {
            var headers = new requestHeaders().get();

            return requester.post(headers, usersServiceUrl + username + '/follow', null);
        }

        function unfollowUser(username) {
            var headers = new requestHeaders().get();

            return requester.remove(headers, usersServiceUrl + username + '/unfollow');
        }

        function makeABlast(blast) {
            var headers = new requestHeaders().get();

            return requester.post(headers, constants.BASE_URL + 'blasts', blast);
        }

        /*function previewUser(username) {
            var headers = new requestHeaders().get();

            return requester.get(headers, constants.BASE_URL + username + '/preview');
        }

        function searchUser(data) {
            return $http.get(serviceUrl + 'search?searchTerm=' + data, getConfig());
        }

        function userFollowersPreview(username) {
            var headers = new requestHeaders().get();

            return requester.get(headers, constants.BASE_URL + username + '/followers/preview');
        }*/

        return {
            // Authentication
            register: register,
            login: login,
            logout: logout,

            // User profiles, functionality etc.
            followUser: followUser,
            unfollowUser: unfollowUser,
            userProfile: userProfile,
            userFollowers: userFollowers,
            makeABlast: makeABlast
        }
    });
});