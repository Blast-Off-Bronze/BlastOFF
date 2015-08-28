define(['app'], function (app) {
    'use strict';

    app.factory('storageService', function () {

        function getSessionToken() {
            return sessionStorage.getItem('sessionToken');
        }

        function setSessionToken(sessionToken) {
            sessionStorage.setItem('sessionToken', sessionToken);
        }

        function isLogged() {
            if (getSessionToken()) {
                return true;
            }

            return false;
        }

        function clearStorage() {
            sessionStorage.clear();
        }

        return {
            getSessionToken: getSessionToken,
            setSessionToken: setSessionToken,

            isLogged: isLogged,

            clearStorage: clearStorage
        }
    });
});