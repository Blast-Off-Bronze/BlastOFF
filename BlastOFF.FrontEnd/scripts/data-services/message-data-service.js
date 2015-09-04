define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('messageDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'messages/';

        function deleteMessage(message) {

            var headers = new requestHeaders().get();

            return requester.delete(headers, serviceUrl + message.Id);
        }

        return {
            deleteMessage: deleteMessage
        }
    });
});