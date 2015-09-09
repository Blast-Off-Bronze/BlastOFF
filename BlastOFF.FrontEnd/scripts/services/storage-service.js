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

        function setUserDetails(user, wantsToBeRemembered){
            if (wantsToBeRemembered) {
                localStorage.setItem('user', user);
            }
            else {
                sessionStorage.setItem('user', user);
            }
        }

        function getUserDetails() {
            if (localStorage.getItem('user')) {
                return localStorage.getItem('user');
            }

            return sessionStorage.getItem('user');
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

            setUserDetails: setUserDetails,
            getUserDetails: getUserDetails,

            isLogged: isLogged,

            clearStorage: clearStorage
        }
    });
});