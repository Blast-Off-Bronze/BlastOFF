define(['app', 'constants', 'request-headers', 'requester'], function (app) {
    'use strict';

    app.factory('blastDataService', function (constants, requestHeaders, requester) {
        var serviceUrl = constants.BASE_URL + 'blasts';

        function getAllBlasts() {

            var headers = new requestHeaders().get();
            return requester.get(headers, serviceUrl);
        }

        // PAGED BLASTS
        function getPagedBlasts(username, startBlastId, pageSize) {

            var startBlastId = startBlastId || '';
            var pageSize = pageSize || constants.DEFAULT_BLAST_FEED_PAGE_SIZE;

            var headers = new requestHeaders().get();

            var url = serviceUrl + username + '/wall/?StartPostId=' + startBlastId + '&PageSize=' + pageSize;

            return requester.get(headers, url);
        }

        return {
            getAllBlasts: getAllBlasts,

            getPagedBlasts: getPagedBlasts
        }
    });
});