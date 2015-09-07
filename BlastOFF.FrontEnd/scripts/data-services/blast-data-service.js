define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('blastDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'blasts';

        function getAllBlasts() {

            var headers = new requestHeaders().get();
            alert("yooo");
            return requester.get(headers, serviceUrl);
        }


        return {
            getAllBlasts: getAllBlasts
        }
    });
});