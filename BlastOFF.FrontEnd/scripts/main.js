requirejs.config({
    baseUrl: 'scripts',

    paths: {
        // Libraries
        'angular': '../libraries/angular/angular.min',
        'angular-messages': '../libraries/angular/angular-messages.min',
        'angular-route': '../libraries/angular/angular-route.min',
        'angularAMD': '../libraries/angularAMD/angularAMD.min',
        'jQuery': '../libraries/jquery/jquery.min',
        'noty': '../libraries/noty/jquery.noty.packaged.min',
        'bootstrap': '../libraries/bootstrap/bootstrap.min',

        // Controllers
        'home-controller': 'controllers/home-controller',
        'header-controller': 'controllers/header-controller',
        'registration-controller': 'controllers/registration-controller',
        'login-controller': 'controllers/logins-controller',
        'music-controller': 'controllers/music-controller',
        'image-controller': 'controllers/image-controller',
        'blast-controller': 'controllers/blast-controller',
        'user-profile-controller': 'controllers/user-profile-controller',

        // Data Services
        'user-data-service': 'data-services/user-data-service',
        'music-data-service': 'data-services/music-data-service',
        'image-data-service': 'data-services/image-data-service',
        'comment-data-service': 'data-services/comment-data-service',
        'message-data-service': 'data-services/message-data-service',
        'blast-data-service': 'data-services/blast-data-service',

        // Directives
        'imageUpload': 'directives/ImageUploadDirective',
        'coverImageUpload' : 'directives/CoverImageUploadDirective',
        'songUpload' : 'directives/SongUploadDirective',
        'infiniteScroll': 'directives/ng-infinite-scroll.min',

        // Models
        'constants': 'models/constants',
        'request-headers': 'models/request-headers',
        'requester': 'models/requester',

        // Services
        'escape-special-chars-service': 'services/escape-special-chars-service',
        'file-reader-service':'services/file-reader-service',
        'music-validation-service':'services/music-validation-service',
        'image-validation-service':'services/image-validation-service',
        'notification-service': 'services/notification-service',
        'storage-service': 'services/storage-service',
        'redirection-service': 'services/redirection-service',

        // App
        'app': 'app'
    },

    shim: {
        'angular': {
            'exports': 'angular'
        },
        'angular-messages': ['angular'],
        'angular-route': ['angular'],
        'angularAMD': ['angular'],
        'noty': ['jQuery'],
        'bootstrap': ['jQuery'],
        'infiniteScroll': ['angular']
    },

    deps: ['app', 'bootstrap', 'header-controller', 'music-controller']
});