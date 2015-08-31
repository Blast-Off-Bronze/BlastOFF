define(['app'], function (app) {
    'use strict';

    return app.constant('constants', {
        // Urls
        'BASE_URL': 'http://localhost:1505/api/',

        // Request Headers
        'CACHE_CONTROL': 'no-cache',
        'CONTENT_TYPE': 'application/json;charset=UTF-8',

        // Messages
        'SUCCESSFUL_REGISTRATION_MESSAGE': 'You were successfully registered as ',
        'SUCCESSFUL_LOGIN_MESSAGE': 'You were successfully logged in as ',
        'SUCCESSFUL_LOGOUT_MESSAGE': 'You were successfully logged out.',

        'INVALID_SONG_FORMAT_MESSAGE': 'Invalid song format.' + '<br/>' + 'Allowed format is ',
        'INVALID_SONG_SIZE_MESSAGE': 'Invalid song size.' + '<br/>' + 'The song size cannot exceed more than ',

        // Miscellaneous
        'DEFAULT_IMAGE_FORMAT': '.mp3.',
        'DEFAULT_FILE_SIZE_UNIT': ' mB.'
    });
});