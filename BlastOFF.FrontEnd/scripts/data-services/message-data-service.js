define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('messageDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'messages/';

        function deleteMessage(message) {

            var headers = new requestHeaders().get();

            return requester.remove(headers, serviceUrl + message.Id);
        }

        function composeMessage(message){
            var headers = new requestHeaders().get();

            return requester.post(headers, serviceUrl, message);
        }

        return {
            deleteMessage: deleteMessage,
            composeMessage: composeMessage
        }
    });
});