define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('musicDataService', function (constants, requestHeaders, requester) {
        //var serviceUrl = constants.BASE_URL + 'account/';
        //var registrationUrl = serviceUrl + 'register';
        //var loginUrl = serviceUrl + 'login';
        //var logoutUrl = serviceUrl + 'logout';

        function addSong(song) {

            var headers = new requestHeaders().get();
            var url = constants.BASE_URL + 'songs';

            return requester.post(headers, url, song);
        }

        return {
            addSong: addSong
        }
    });
});