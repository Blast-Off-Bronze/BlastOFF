define(['app'], function (app) {
    'use strict';

    app.factory('requester', function ($q, $http) {

        function Requester() {
            this.post = send_POST_request;
            this.get = send_GET_request;
            this.put = send_PUT_request;
            this.remove = send_DELETE_request;
        }

        function request(method, headers, url, data) {
            var deferrer = $q.defer();

            $http({
                method: method,
                headers: headers,
                url: url,
                data: data
            })
                .success(deferrer.resolve)
                .error(deferrer.reject);

            return deferrer.promise;
        }

        function send_POST_request(headers, url, data) {
            return request('POST', headers, url, data);
        }

        function send_GET_request(headers, url) {
            return request('GET', headers, url);
        }

        function send_PUT_request(headers, url, data) {
            return request('PUT', headers, url, data);
        }

        function send_DELETE_request(headers, url) {
            return request('DELETE', headers, url);
        }

        return new Requester();
    });
});