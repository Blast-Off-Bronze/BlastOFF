define(['app', 'storage-service', 'constants'], function (app) {
    'use strict';

    app.factory('requestHeaders', function (storageService, constants) {

        function RequestHeaders() {
            this.cacheControl = constants.CACHE_CONTROL;
            this.contentType = constants.CONTENT_TYPE;
        }

        RequestHeaders.prototype.get = function () {
            var headers = {
                'Cache-Control': this.cacheControl,
                'Content-Type': this.contentType
            };

            if (storageService.isLogged()) {
                headers['Authorization'] = storageService.getSessionToken();
            }

            return headers;
        };

        return RequestHeaders;
    });
});