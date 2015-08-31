define(['app'], function (app) {
    'use strict';

    app.factory('storageService', function () {

        function getSessionToken() {
            if (localStorage.getItem('sessionToken')) {
                return localStorage.getItem('sessionToken');
            }

            return sessionStorage.getItem('sessionToken');
        }

        function setSessionToken(sessionToken, wantsToBeRemembered) {
            if (wantsToBeRemembered) {
                localStorage.setItem('sessionToken', sessionToken);
            }
            else {
                sessionStorage.setItem('sessionToken', sessionToken);
            }
        }

        function isLogged() {
            if (getSessionToken()) {
                return true;
            }
            return false;
        }

        function clearStorage() {
            sessionStorage.clear();
            localStorage.clear();
        }

        return {
            getSessionToken: getSessionToken,
            setSessionToken: setSessionToken,

            isLogged: isLogged,

            clearStorage: clearStorage
        }
    });
});