define(['app'], function (app) {
    'use strict';

    return app.constant('constants', {
        // Urls
        'BASE_URL': 'http://localhost:1505/api/',

        // Request Headers
        'CACHE_CONTROL': 'no-cache',
        'CONTENT_TYPE': 'application/json;charset=UTF-8',

        // Numbers
        'COVER_IMAGE_SIZE_LIMIT': 1024, // 1 mB
        'SONG_SIZE_LIMIT': 20480, // 20 mB
        'BYTE_SIZE': 1024,
        'DEFAULT_REDIRECTION_TIMEOUT': 1000,

        // Messages
        'SUCCESSFUL_REGISTRATION_MESSAGE': 'You were successfully registered as ',
        'SUCCESSFUL_LOGIN_MESSAGE': 'You were successfully logged in as ',
        'SUCCESSFUL_LOGOUT_MESSAGE': 'You were successfully logged out.',

        'INVALID_SONG_FORMAT_MESSAGE': 'Invalid song format.' + '<br/>' + 'Allowed format is ',
        'INVALID_SONG_SIZE_MESSAGE': 'Invalid song size.' + '<br/>' + 'The song size cannot exceed more than ',

        'INVALID_COVER_IMAGE_FORMAT_MESSAGE': 'Invalid cover image format.' + '<br/>' + 'Allowed format is ',
        'INVALID_COVER_IMAGE_SIZE_MESSAGE': 'Invalid cover image size.' + '<br/>' + 'The cover image size cannot exceed more than ',


        // Miscellaneous
        'DEFAULT_SONG_FORMAT': '.mp3.',
        'DEFAULT_COVER_IMAGE_FORMAT': '.jpg',
        'DEFAULT_FILE_SIZE_UNIT': ' kB.'
    });
});