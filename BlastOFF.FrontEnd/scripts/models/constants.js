define(['app'], function (app) {
    'use strict';

    return app.constant('constants', {
        // Urls
        'BASE_URL': 'http://localhost:1505/api/',

        // Request Headers
        'CACHE_CONTROL': 'no-cache',
        'CONTENT_TYPE': 'application/json;charset=UTF-8'
    });
});