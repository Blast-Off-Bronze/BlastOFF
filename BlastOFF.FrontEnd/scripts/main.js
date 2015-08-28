requirejs.config({
    baseUrl: 'scripts',

    paths: {
        // Libraries
        'angular': '../libraries/angular/angular.min',
        'angular-messages': '../libraries/angular/angular-messages.min',
        'angular-route': '../libraries/angular/angular-route.min',
        'angularAMD': '../libraries/angularAMD/angularAMD.min',
        'jquery': '../libraries/jquery/jquery.min',
        'noty': '../libraries/noty/jquery.noty.packaged.min',

        // Controllers
        'registration-controller': 'controllers/registration-controller',
        'login-controller': 'controllers/logins-controller',

        // Data Services
        'user-data-service': 'data-services/user-data-service',

        // Models
        'constants': 'models/constants',
        'request-headers': 'models/request-headers',
        'requester': 'models/requester',

        // Services
        'storage-service': 'services/storage-service',

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
        'noty': ['jquery']
    },

    deps: ['app', 'jquery']
});