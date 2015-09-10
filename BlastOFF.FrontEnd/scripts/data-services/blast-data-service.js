define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('blastDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'blasts/';

        function getPublicBlasts(currentPage, pageSize) {
            var currentPage = currentPage || constants.DEFAULT_CURRENT_PAGE;
            var pageSize = pageSize || constants.DEFAULT_BLAST_FEED_PAGE_SIZE;

            var headers = new requestHeaders().get();

            var url = serviceUrl + '/public/?CurrentPage=' + currentPage + '&PageSize=' + pageSize;

            return requester.get(headers, url);
        }

        function likeBlast(blast) {

            var headers = new requestHeaders().get();

            return requester.post(headers, serviceUrl + blast.id + '/like', null);
        }

        function unlikeBlast(blast) {

            var headers = new requestHeaders().get();

            return requester.remove(headers, serviceUrl + blast.id + '/unlike');
        }

        function commentBlast(blast) {
            var headers = new requestHeaders().get();

            return requester.post(headers, serviceUrl + blast.id + '/comments', blast.commentModel);
        }

        return {
            getPublicBlasts: getPublicBlasts,

            likeBlast: likeBlast,
            unlikeBlast: unlikeBlast,

            commentBlast: commentBlast
        }
    });
});