define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('commentDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'comments/';

        function modifyComment(comment) {

            var headers = new requestHeaders().get();

            return requester.put(headers, serviceUrl + comment.id, comment);
        }

        function deleteComment(comment) {

            var headers = new requestHeaders().get();

            return requester.remove(headers, serviceUrl + comment.id);
        }

        function likeComment(comment) {

            var headers = new requestHeaders().get();

            return requester.post(headers, serviceUrl + comment.id + '/like', null);
        }

        function unlikeComment(comment) {

            var headers = new requestHeaders().get();

            return requester.remove(headers, serviceUrl + comment.id + '/unlike');
        }

        return {
            modifyComment: modifyComment,
            deleteComment: deleteComment,
            likeComment: likeComment,
            unlikeComment: unlikeComment
        }
    });
});